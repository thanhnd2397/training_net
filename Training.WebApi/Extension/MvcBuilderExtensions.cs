using Microsoft.AspNetCore.Mvc;
using Training.Application.Common.Interfaces;

namespace Training.WebApi.Extension
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddCustomValidationResponse(this IMvcBuilder builder)
        {
            builder.ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errorCode = context.ModelState
                        .SelectMany(x => x.Value.Errors)
                        .Select(e => e.ErrorMessage)
                        .FirstOrDefault() ?? "ERR_VALIDATION_FAILED";

                    var messageService = context.HttpContext.RequestServices.GetService<IMessageService>();
                    var message = messageService?.GetMessage(errorCode) ?? "Validation failed.";

                    var result = new
                    {
                        code = "VALIDATION_ERR",
                        message,
                        time = DateTime.UtcNow.ToString("o")
                    };

                    return new BadRequestObjectResult(result);
                };
            });

            return builder;
        }
    }
}