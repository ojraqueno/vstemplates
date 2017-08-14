using FluentValidation;
using Microsoft.AspNet.Identity.Owin;
using MVC5_R.Data;
using MVC5_R.Infrastructure.Identity;
using MVC5_R.Infrastructure.Logging;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MVC5_R
{
    public class DependencyConfig
    {
        public static Container Container { get; private set; }

        public static void Configure()
        {
            Container = new Container();
            Container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();    

            Container.Register<ApplicationDbContext, ApplicationDbContext>(Lifestyle.Scoped);
            Container.Register(typeof(IValidator<>), new[] { Assembly.GetExecutingAssembly() });
            Container.Register<ApplicationSignInManager>(GetApplicationSignInManager, Lifestyle.Scoped);
            Container.Register<ApplicationUserManager>(GetApplicationUserManager, Lifestyle.Scoped);
            Container.Register<IMVCLogger, MVCLogger>(Lifestyle.Singleton);

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(Container));
        }

        private static ApplicationSignInManager GetApplicationSignInManager()
        {
            return HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
        }

        private static ApplicationUserManager GetApplicationUserManager()
        {
            return HttpContext.Current.GetOwinContext().Get<ApplicationUserManager>();
        }
    }
}