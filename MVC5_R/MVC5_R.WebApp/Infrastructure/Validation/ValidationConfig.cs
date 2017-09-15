using FluentValidation.Mvc;
using MVC5_R.WebApp.Infrastructure.Dependency;

namespace MVC5_R.WebApp.Infrastructure.Validation
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