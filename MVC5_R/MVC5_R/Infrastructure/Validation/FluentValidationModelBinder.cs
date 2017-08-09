using FluentValidation;
using System;
using System.Web.Mvc;

namespace MVC5_R.Infrastructure.Validation
{
    public class FluentValidationModelBinder : DefaultModelBinder
    {
        private readonly IServiceProvider serviceProvider;

        public FluentValidationModelBinder(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = base.BindModel(controllerContext, bindingContext);

            FluentValidate(bindingContext, model);

            return model;
        }

        private void FluentValidate(ModelBindingContext bindingContext, object model)
        {
            var validator = serviceProvider.GetService(typeof(IValidator<>).MakeGenericType(bindingContext.ModelType));
            if (validator == null) return;
            
            var concreteValidator = validator as IValidator;

            var validationResult = concreteValidator.Validate(model);
            if (validationResult.IsValid) return;                
            
            foreach (var error in validationResult.Errors)
            {
                bindingContext.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }
    }
}