using FluentValidation;
using MVC5_R.Data;
using MVC5_R.Infrastructure.Logging;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using System.Reflection;
using System.Web.Mvc;

namespace MVC5_R
{
    public class DependencyConfig
    {
        public static Container Container { get; private set; }

        public static void Configure()
        {
            Container = new Container();
            
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(Container));

            Container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            var assemblyOfValidationClasses = Assembly.GetExecutingAssembly();
            Container.Register(typeof(IValidator<>), new[] { assemblyOfValidationClasses });

            Container.Register<ApplicationDbContext, ApplicationDbContext>(Lifestyle.Scoped);
            Container.Register<IMVCLogger, MVCLogger>(Lifestyle.Singleton);
        }
    }
}