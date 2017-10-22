using Microsoft.AspNet.Identity;
using MVC5_R.Infrastructure.Data;
using MVC5_R.Models;
using MVC5_R.WebApp.Infrastructure.Dependency;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5_R.WebApp.Infrastructure.Security
{
    public class AuthorizePermissionAttribute : AuthorizeAttribute
    {
        private readonly ApplicationDbContext _db;

        public AuthorizePermissionAttribute(Permission permission)
        {
            _db = DependencyConfig.Instance.Container.GetInstance<ApplicationDbContext>();
            Permission = permission;
        }

        public Permission Permission { get; private set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized) return false;

            var currentUserId = httpContext.User.Identity.GetUserId();

            var currentUser = _db.Users
                .Include(u => u.CustomRoles)
                .SingleOrDefault(u => u.Id == currentUserId);

            return currentUser
                .CustomRoles
                .Any(cr => cr.HasPermission(Permission));
        }
    }
}