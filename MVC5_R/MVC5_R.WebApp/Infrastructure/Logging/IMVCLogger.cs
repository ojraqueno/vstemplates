using System.Web.Mvc;

namespace MVC5_R.WebApp.Infrastructure.Logging
{
    public interface IMVCLogger
    {
        void Log(ExceptionContext filterContext);
    }
}