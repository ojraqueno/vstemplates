using Microsoft.AspNet.Identity;
using MVC5_R;
using MVC5_R.Infrastructure.Data;
using MVC5_R.Models;
using System.Data.Entity;
using System.Linq;

namespace System.Security.Principal
{
    public static class IIdentityExtensions
    {
        ///// <summary>
        ///// Gets the currently logged in user.
        ///// </summary>
        ///// <param name="identity"></param>
        ///// <returns></returns>
        //public static ApplicationUser GetCurrentUser(this IIdentity identity)
        //{
        //    return identity.GetCurrentUser(null);
        //}

        ///// <summary>
        ///// Gets the currently logged in user.
        ///// </summary>
        ///// <param name="identity"></param>
        ///// <param name="includes">A comma-separated list of navigation properties to include.</param>
        ///// <returns></returns>
        //public static ApplicationUser GetCurrentUser(this IIdentity identity, string includes)
        //{
        //    var db = DependencyConfig.Instance.Container.GetInstance<ApplicationDbContext>();
        //    var userQuery = db.Users.AsQueryable();
        //    var currentUserId = identity.GetUserId();

        //    if (!String.IsNullOrWhiteSpace(includes))
        //    {
        //        foreach (var include in includes.Split(','))
        //        {
        //            userQuery = userQuery.Include(include);
        //        }
        //    }

        //    return userQuery.Single(u => u.Id == currentUserId);
        //}
    }
}