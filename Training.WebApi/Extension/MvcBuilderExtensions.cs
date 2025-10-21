namespace Training.WebApi.Extension;

public static class MvcBuilderExtensions
{
    public static IMvcBuilder AddCustomValidationResponse(this IMvcBuilder builder)
    {
        builder.ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                // Lấy lỗi đầu tiên
                var firstError = context.ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .SelectMany(x => x.Value!.Errors.Select(e => new
                    {
                        Field = x.Key,
                        Code = e.ErrorMessage,
                    }))
                    .FirstOrDefault();

                var errorCode = firstError?.Code ?? "ERR_VALIDATION_FAILED";
                var field = firstError?.Field ?? "";

                // Lấy message từ service nếu có
                var messageService = context.HttpContext.RequestServices.GetService<IMessageService>();
                var messageTemplate = messageService?.GetMessage(errorCode) ?? firstError?.Code ?? "Validation failed.";

                // Replace tất cả placeholder {PropertyName}, {MinLength}, {MaxLength}, {MinValue}, {MaxValue}, ...
                var message = messageTemplate.Replace("{PropertyName}", field.Split('.').LastOrDefault() ?? "");

                return new BadRequestObjectResult(new
                {
                    code = "VALIDATION_ERR",
                    message,
                    time = DateTime.UtcNow.ToString("o")
                });
            };
        });

        return builder;
    }
}