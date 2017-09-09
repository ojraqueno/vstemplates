﻿using MVC5_R.WebApp.Infrastructure.Mvc;
using System.Web.Mvc;

namespace MVC5_R.WebApp.Controllers
{
    public class HomeController : AppController
    {
        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
    }
}