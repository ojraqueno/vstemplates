using MVC5_R.WebApp.Infrastructure.Logging;
using System.Web.Mvc;

namespace MVC5_R.WebApp.Infrastructure.Mvc
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AuthorizeAttribute());
            filters.Add(new ExceptionLoggerAttribute());
        }
    }
}