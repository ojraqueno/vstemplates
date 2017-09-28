using System.Web.Http.Filters;
using System.Web.Mvc;

namespace MVC5_R.WebAPI.Infrastructure.WebAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection mvcFilters, HttpFilterCollection webApiFilters)
        {
            mvcFilters.Add(new HandleErrorAttribute());

            webApiFilters.Add(new System.Web.Http.AuthorizeAttribute());
        }
    }
}