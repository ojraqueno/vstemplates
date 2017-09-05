using MVC5_R.WebApp.Infrastructure.Logging;
using System.Web.Mvc;

namespace MVC5_R.WebApp.Infrastructure.Startup
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ExceptionLoggerAttribute());
        }
    }
}