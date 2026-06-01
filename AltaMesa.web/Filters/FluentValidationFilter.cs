using FluentValidation;
using System;
using System.Linq;
using System.Web.Mvc;

namespace AltaMesa.web.Filters
{
    public class FluentValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            foreach (var argument in filterContext.ActionParameters.Values)
            {
                if (argument == null) continue;

                var argumentType = argument.GetType();
                var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);
                var validator = DependencyResolver.Current.GetService(validatorType) as IValidator;
                if (validator == null) continue;

                var contextType = typeof(ValidationContext<>).MakeGenericType(argumentType);
                var context = (IValidationContext)Activator.CreateInstance(contextType, argument);
                var result = validator.Validate(context);
                if (result.IsValid) continue;

                foreach (var error in result.Errors)
                {
                    filterContext.Controller.ViewData.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}
