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
            EnsureSuperAdminSeeded(userManager);
        }

        private static void EnsureRolesSeeded(AppDbContext db, RoleManager<AppIdentityRole> roleManager)
        {
            var roleNames = new string[] { RoleNames.SuperAdmin };

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
                    case RoleNames.SuperAdmin:
                        permissions = PermissionHelper.ApplicationAdminPermissions;
                        break;

                    default:
                        break;
                }

                role.PermissionsString = String.Empty;
                role.AddPermissions(permissions);
                db.SaveChanges();
            }
        }

        private static void EnsureSuperAdminSeeded(UserManager<AppIdentityUser> userManager)
        {
            var superAdminUsernames = new List<string> { "core1@gmail.com" };

            foreach (var username in superAdminUsernames)
            {
                var defaultAdmin = userManager.FindByNameAsync(username).ConfigureAwait(false).GetAwaiter().GetResult();
                if (defaultAdmin != null) return;

                var user = new AppIdentityUser { AddedOn = DateTime.UtcNow, UserName = username, Email = username, EmailConfirmed = true, TimezoneOffsetMinutes = TimezoneOffsets.Philippines };
                var createUserResult = userManager.CreateAsync(user, "password123").ConfigureAwait(false).GetAwaiter().GetResult();
                if (!createUserResult.Succeeded) throw new Exception("Failed to create seed user!");

                var addToRoleResult = userManager.AddToRoleAsync(user, RoleNames.SuperAdmin).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }
    }
}