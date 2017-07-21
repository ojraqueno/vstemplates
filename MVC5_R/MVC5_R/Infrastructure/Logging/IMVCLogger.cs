using System.Web.Mvc;

namespace MVC5_R.Infrastructure.Logging
{
    public interface IMVCLogger
    {
        void Log(ExceptionContext filterContext);
    }
}