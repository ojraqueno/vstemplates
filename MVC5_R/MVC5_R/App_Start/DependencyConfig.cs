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
using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MVC5_R
{
    public class DependencyConfig : IServiceProvider
    {
        private static readonly Lazy<DependencyConfig> lazy = new Lazy<DependencyConfig>(() => new DependencyConfig());

        public static DependencyConfig Instance { get { return lazy.Value; } }

        public Container Container { get; private set; }

        private DependencyConfig()
        {
            Container = ConfigureContainer();
        }

        public Container ConfigureContainer()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            container.Register<ApplicationDbContext, ApplicationDbContext>(Lifestyle.Scoped);
            RegisterValidators(container);
            RegisterMediatR(container);
            container.Register<ApplicationSignInManager>(GetApplicationSignInManager, Lifestyle.Scoped);
            container.Register<ApplicationUserManager>(GetApplicationUserManager, Lifestyle.Scoped);
            container.Register<IMVCLogger, MVCLogger>(Lifestyle.Singleton);

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            return container;
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
        private static void RegisterMediatR(Container container)
        {
            var assemblyOfMediatRClasses = Assembly.GetExecutingAssembly();
            var assemblies = new[] { assemblyOfMediatRClasses };
            container.RegisterSingleton<IMediator, Mediator>();
            container.Register(typeof(IRequestHandler<,>), assemblies);
            container.Register(typeof(IAsyncRequestHandler<,>), assemblies);
            container.Register(typeof(IRequestHandler<>), assemblies);
            container.Register(typeof(IAsyncRequestHandler<>), assemblies);
            container.Register(typeof(ICancellableAsyncRequestHandler<>), assemblies);
            container.RegisterCollection(typeof(INotificationHandler<>), assemblies);
            container.RegisterCollection(typeof(IAsyncNotificationHandler<>), assemblies);
            container.RegisterCollection(typeof(ICancellableAsyncNotificationHandler<>), assemblies);

            //Pipeline
            container.RegisterCollection(typeof(IPipelineBehavior<,>), new[]
            {
                typeof(RequestPreProcessorBehavior<,>),
                typeof(RequestPostProcessorBehavior<,>)
            });

            container.RegisterSingleton(new SingleInstanceFactory(container.GetInstance));
            container.RegisterSingleton(new MultiInstanceFactory(container.GetAllInstances));
        }

        private static void RegisterValidators(Container container)
        {
            var assemblyOfValidationClasses = Assembly.GetExecutingAssembly();
            container.Register(typeof(IValidator<>), new[] { Assembly.GetExecutingAssembly() });
        }

        public object GetService(Type serviceType)
        {
            return ((IServiceProvider)Container).GetService(serviceType);
        }
    }
}