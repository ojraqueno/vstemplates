using FluentValidation.Mvc;
using MVC5_R.WebApp.Infrastructure.Validation;

namespace MVC5_R.WebApp.Infrastructure.Startup
{
    public class ValidationConfig
    {
        public static void Configure()
        {
            var injector = DependencyConfig.Instance;
            FluentValidationModelValidatorProvider.Configure(
                provider =>
                {
                    provider.ValidatorFactory = new FluentValidatorFactory(injector);
                }
            );
        }
    }
}