using Microsoft.AspNet.Identity;
using MVC5_R.Infrastructure.Data;
using MVC5_R.Infrastructure.Logging;
using MVC5_R.Models;
using System;
using System.Web.Mvc;

namespace MVC5_R.WebApp.Infrastructure.Logging
{
    public class MVCLogger : IMVCLogger
    {
        public void Log(ExceptionContext filterContext)
        {
            var logEntry = new LogEntry
            {
                Action = Convert.ToString(filterContext.RouteData.Values["action"]),
                Controller = Convert.ToString(filterContext.RouteData.Values["controller"]),
                LoggedOn = DateTime.UtcNow,
                Level = LogLevel.Error,
                Message = filterContext.Exception.Message,
                Request = GetRequest(filterContext),
                StackTrace = filterContext.Exception.StackTrace,
                UserId = filterContext.HttpContext.User.Identity.GetUserId()
            };

            Logger.Log(logEntry);
        }

        private static string GetRequest(ExceptionContext filterContext)
        {
            var headers = filterContext.HttpContext.Request.ServerVariables["ALL_RAW"].Replace("\r\n", Environment.NewLine);
            var form = filterContext.HttpContext.Request.Form.ToString();

            return headers + Environment.NewLine + form;
        }
    }
}