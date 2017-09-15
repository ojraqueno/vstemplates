﻿using MVC5_R.WebApp.Infrastructure.Bundling;
using MVC5_R.WebApp.Infrastructure.Mvc;
using MVC5_R.WebApp.Infrastructure.Validation;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MVC5_R.WebApp
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            ValidationConfig.Configure();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}