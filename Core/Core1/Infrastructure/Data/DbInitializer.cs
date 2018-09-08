using Core1.Infrastructure.Identity;
using Core1.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core1.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext db, UserManager<AppIdentityUser> userManager, RoleManager<AppIdentityRole> roleManager)
        {
            EnsureRolesSeeded(db, roleManager);
            EnsureApplicationAdminSeeded(userManager);
        }

        private static void EnsureRolesSeeded(AppDbContext db, RoleManager<AppIdentityRole> roleManager)
        {
            var roleNames = new string[] { RoleNames.ApplicationAdmin, RoleNames.CustomerAdmin };

            foreach (string roleName in roleNames)
            {
                if (!roleManager.RoleExistsAsync(roleName).ConfigureAwait(false).GetAwaiter().GetResult())
                {
                    roleManager.CreateAsync(new AppIdentityRole(roleName)).ConfigureAwait(false).GetAwaiter().GetResult();
                }

                var role = db.Roles.Single(r => r.Name == roleName);
                IList<Permission> permissions = new List<Permission>();
                switch (roleName)
                {
                    case RoleNames.ApplicationAdmin:
                        permissions = PermissionHelper.ApplicationAdminPermissions;
                        break;

                    case RoleNames.CustomerAdmin:
                        permissions = PermissionHelper.CustomerAdminPermissions;
                        break;

                    default:
                        break;
                }

                role.PermissionsString = String.Empty;
                role.AddPermissions(permissions);
                db.SaveChanges();
            }
        }

        private static void EnsureApplicationAdminSeeded(UserManager<AppIdentityUser> userManager)
        {
            var defaultAdmin = userManager.FindByNameAsync("core1@email.com").ConfigureAwait(false).GetAwaiter().GetResult();
            if (defaultAdmin != null) return;

            var user = new AppIdentityUser { UserName = "core1@email.com", Email = "core1@email.com" };
            var createUserResult = userManager.CreateAsync(user, "password123").ConfigureAwait(false).GetAwaiter().GetResult();
            if (!createUserResult.Succeeded) throw new Exception("Failed to create seed user!");

            var addToRoleResult = userManager.AddToRoleAsync(user, RoleNames.ApplicationAdmin).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}