using Microsoft.AspNet.Identity;
using MVC5_R.WebApp.Infrastructure.Data;
using MVC5_R.WebApp.Models;
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
                Message = filterContext.Exception.Message,
                StackTrace = filterContext.Exception.StackTrace,
                UserId = filterContext.HttpContext.User.Identity.GetUserId()
            };

            using (var db = new ApplicationDbContext())
            {
                db.LogEntries.Add(logEntry);
                db.SaveChanges();
            }
        }
    }
}