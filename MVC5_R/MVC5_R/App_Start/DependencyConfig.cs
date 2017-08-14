using FluentValidation;
using MediatR;
using MediatR.Pipeline;
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
            RegisterValidators();
            RegisterMediatR();
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

        // More info: https://github.com/jbogard/MediatR/blob/master/samples/MediatR.Examples.SimpleInjector/Program.cs
        private static void RegisterMediatR()
        {
            var assemblyOfMediatRClasses = Assembly.GetExecutingAssembly();
            var assemblies = new[] { assemblyOfMediatRClasses };
            Container.RegisterSingleton<IMediator, Mediator>();
            Container.Register(typeof(IRequestHandler<,>), assemblies);
            Container.Register(typeof(IAsyncRequestHandler<,>), assemblies);
            Container.Register(typeof(IRequestHandler<>), assemblies);
            Container.Register(typeof(IAsyncRequestHandler<>), assemblies);
            Container.Register(typeof(ICancellableAsyncRequestHandler<>), assemblies);
            Container.RegisterCollection(typeof(INotificationHandler<>), assemblies);
            Container.RegisterCollection(typeof(IAsyncNotificationHandler<>), assemblies);
            Container.RegisterCollection(typeof(ICancellableAsyncNotificationHandler<>), assemblies);

            //Pipeline
            Container.RegisterCollection(typeof(IPipelineBehavior<,>), new[]
            {
                typeof(RequestPreProcessorBehavior<,>),
                typeof(RequestPostProcessorBehavior<,>)
            });

            Container.RegisterSingleton(new SingleInstanceFactory(Container.GetInstance));
            Container.RegisterSingleton(new MultiInstanceFactory(Container.GetAllInstances));
        }

        private static void RegisterValidators()
        {
            var assemblyOfValidationClasses = Assembly.GetExecutingAssembly();
            Container.Register(typeof(IValidator<>), new[] { Assembly.GetExecutingAssembly() });
        }
    }
}