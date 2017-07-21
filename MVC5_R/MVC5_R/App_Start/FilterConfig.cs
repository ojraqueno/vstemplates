using MVC5_R.Infrastructure.Logging;
using System.Web.Mvc;

namespace MVC5_R
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