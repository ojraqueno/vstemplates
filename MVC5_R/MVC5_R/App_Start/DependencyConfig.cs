using MVC5_R.Data;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
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

            Container.Register<ApplicationDbContext, ApplicationDbContext>(Lifestyle.Scoped);
        }
    }
}