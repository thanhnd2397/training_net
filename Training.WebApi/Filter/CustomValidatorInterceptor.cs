using Microsoft.AspNetCore.Mvc.Filters;

namespace Training.WebApi.Filter;

public class CustomValidatorInterceptor : IActionFilter
{
    private readonly IServiceProvider _serviceProvider;

    public CustomValidatorInterceptor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var errors = new List<object>();

        foreach (var arg in context.ActionArguments.Values)
        {
            if (arg == null) continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(arg.GetType());
            var validator = _serviceProvider.GetService(validatorType) as IValidator;

            if (validator == null) continue;

            var result = validator.Validate(new ValidationContext<object>(arg));

            if (!result.IsValid)
            {
                errors.AddRange(result.Errors.Select(x => new
                {
                    Field = x.PropertyName,
                    Message = x.ErrorMessage // FluentValidation >=11: resolve {MinValue}, {MaxValue}, ...
                }));
            }
        }

        if (errors.Any())
        {
            context.Result = new BadRequestObjectResult(new
            {
                code = "VALIDATION_ERR",
                errors,
                time = DateTime.UtcNow.ToString("o")
            });
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}