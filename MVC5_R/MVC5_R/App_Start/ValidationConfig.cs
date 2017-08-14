using FluentValidation.Mvc;
using MVC5_R.Infrastructure.Validation;
using System.Web.Mvc;

namespace MVC5_R
{
    public class ValidationConfig
    {
        public static void Configure()
        {
            ModelBinders.Binders.DefaultBinder = new FluentValidationModelBinder(DependencyConfig.Container);
            FluentValidationModelValidatorProvider.Configure();
        }
    }
}