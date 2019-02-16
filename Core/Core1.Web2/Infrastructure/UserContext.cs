using Core1.Infrastructure.Data;
using Core1.Infrastructure.Identity;
using Core1.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Core1.Web2.Infrastructure
{
    public interface IUserContext
    {
        AppIdentityUser CurrentUser { get; }
        IList<AppIdentityRole> CurrentUserRoles { get; }
        string CurrentUserUsername { get; }

        bool HasPermission(Permission permission);
    }

    public class UserContext : IUserContext
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(AppDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        private AppIdentityUser _currentUser;

        public AppIdentityUser CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    var currentUserId = _httpContextAccessor.HttpContext.User.FindAll(ClaimTypes.NameIdentifier).LastOrDefault()?.Value;

                    if (!String.IsNullOrWhiteSpace(currentUserId))
                    {
                        _currentUser = _db
                            .Users
                            .Single(u => u.Id == currentUserId);
                    }
                }

                return _currentUser;
            }
        }

        private IList<AppIdentityRole> _currentUserRoles;

        public IList<AppIdentityRole> CurrentUserRoles
        {
            get
            {
                if (_currentUserRoles == null)
                {
                    var userRoles = _db
                        .UserRoles
                        .Where(ur => ur.UserId == CurrentUser.Id)
                        .ToList();

                    var userRoleIds = userRoles
                        .Select(ur => ur.RoleId)
                        .ToList();

                    _currentUserRoles = _db
                        .Roles
                        .Where(r => userRoleIds.Contains(r.Id))
                        .ToList();
                }

                return _currentUserRoles;
            }
        }

        private IList<Permission> _currentUserPermissions;

        public IList<Permission> CurrentUserPermissions
        {
            get
            {
                if (_currentUserPermissions == null)
                {
                    _currentUserPermissions = CurrentUserRoles
                        .SelectMany(r => r.Permissions)
                        .ToList();
                }

                return _currentUserPermissions;
            }
        }

        public string CurrentUserUsername => _httpContextAccessor.HttpContext.User.Identity.Name;

        public bool HasPermission(Permission permission) => CurrentUserPermissions.Contains(permission);
    }
}