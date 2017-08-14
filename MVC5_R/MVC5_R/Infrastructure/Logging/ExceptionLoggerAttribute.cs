using System.Web.Mvc;

namespace MVC5_R.Infrastructure.Logging
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