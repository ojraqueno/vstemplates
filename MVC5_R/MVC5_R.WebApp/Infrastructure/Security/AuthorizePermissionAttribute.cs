using Microsoft.AspNet.Identity;
using MVC5_R.Infrastructure.Data;
using MVC5_R.Infrastructure.Logging;
using MVC5_R.Models;
using MVC5_R.WebApp.Infrastructure.Dependency;
using MVC5_R.WebApp.Infrastructure.Logging;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVC5_R.WebApp.Infrastructure.Security
{
    public class AuthorizePermissionAttribute : AuthorizeAttribute
    {
        public AuthorizePermissionAttribute(Permission permission)
        {
            Permission = permission;
        }

        public Permission Permission { get; private set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized) return false;

            var db = DependencyConfig.Instance.Container.GetInstance<ApplicationDbContext>();

            var currentUserId = httpContext.User.Identity.GetUserId();

            var currentUser = db.Users
                .Include(u => u.CustomRoles)
                .SingleOrDefault(u => u.Id == currentUserId);

            var authorizedViaPermission = currentUser
                .CustomRoles
                .Any(cr => cr.HasPermission(Permission));

            if (!authorizedViaPermission)
            {
                var logEntry = LogHelper.CreateLogEntryFromCurrentContext(LogLevel.Warn, "Unauthorized Request");
                Logger.Log(logEntry);
            }

            return authorizedViaPermission;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            filterContext.HttpContext.Response.End();
            filterContext.HttpContext.Response.Close();
        }
    }
}