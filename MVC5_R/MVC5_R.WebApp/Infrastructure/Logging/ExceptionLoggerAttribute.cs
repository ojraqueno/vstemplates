using MVC5_R.WebApp.Infrastructure.Startup;
using System.Web.Mvc;

namespace MVC5_R.WebApp.Infrastructure.Logging
{
    public class ExceptionLoggerAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var logger = DependencyConfig.Instance.Container.GetInstance<IMVCLogger>();
            logger.Log(filterContext);
        }
    }
}