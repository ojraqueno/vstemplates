using Core1.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Core1.Web.Infrastructure
{
    public class AuthorizePermissionAttribute : TypeFilterAttribute
    {
        public AuthorizePermissionAttribute(Permission permission) : base(typeof(AuthorizePermissionFilter))
        {
            Arguments = new object[] { permission };
        }
    }

    public class AuthorizePermissionFilter : IAuthorizationFilter
    {
        private readonly Permission _permission;

        public AuthorizePermissionFilter(Permission permission)
        {
            _permission = permission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userContext = context.HttpContext.RequestServices.GetRequiredService<IUserContext>();

            var hasPermission = userContext.CurrentUser != null && userContext.HasPermission(_permission);

            if (!hasPermission)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}