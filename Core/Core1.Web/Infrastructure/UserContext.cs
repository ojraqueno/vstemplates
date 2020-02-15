using Core1.Infrastructure.Data;
using Core1.Infrastructure.Identity;
using Core1.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core1.Web.Infrastructure
{
    public interface IUserContext
    {
        AppIdentityUser CurrentUser { get; }

        void Clear();

        bool HasPermission(Permission permission);

        Task Initialize(string username);
    }

    public class UserContext : IUserContext
    {
        private readonly AppDbContext _db;
        private IList<Permission> _currentUserPermissions;

        public UserContext(AppDbContext db)
        {
            _db = db;
        }

        public AppIdentityUser CurrentUser { get; private set; }

        public void Clear()
        {
            CurrentUser = null;
            _currentUserPermissions = new List<Permission>();
        }

        public bool HasPermission(Permission permission) => _currentUserPermissions.Contains(permission);

        public async Task Initialize(string username)
        {
            if (String.IsNullOrWhiteSpace(username)) throw new ArgumentNullException(nameof(username));

            CurrentUser = await _db
                .Users
                .SingleAsync(u => u.UserName == username);

            var currentUserRoles = GetUserRoles(CurrentUser.Id);

            _currentUserPermissions = currentUserRoles
                .SelectMany(r => r.Permissions)
                .ToList();
        }

        private IList<AppIdentityRole> GetUserRoles(string userId)
        {
            if (String.IsNullOrWhiteSpace(userId)) throw new ArgumentNullException(nameof(userId));

            var userRoles = _db
                .UserRoles
                .Where(ur => ur.UserId == userId)
                .ToList();

            var userRoleIds = userRoles
                .Select(ur => ur.RoleId)
                .ToList();

            var currentUserRoles = _db
                .Roles
                .Where(r => userRoleIds.Contains(r.Id))
                .ToList();

            return currentUserRoles;
        }
    }
}