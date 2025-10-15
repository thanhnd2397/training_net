using System.Net;
using Serilog;
using Training.Application.Common.Exceptions;
using Training.Application.Common.Interfaces;

namespace Training.WebApi.Filter;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMessageService _messageService;

    public GlobalExceptionMiddleware(RequestDelegate next, IMessageService messageService)
    {
        _next = next;
        _messageService = messageService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        Log.Error(ex, "Unhandled exception occurred.");

        context.Response.ContentType = "application/json";

        string errorCode;
        string message;
        int statusCode;

        switch (ex)
        {
            case UnauthorizedAccessException:
                statusCode = (int)HttpStatusCode.Unauthorized;
                errorCode = "ERR_AUTH_REQUIRED";
                message = _messageService.GetMessage(errorCode) ?? "Unauthorized";
                break;

            case BadRequestException notFoundEx:
                statusCode = (int)HttpStatusCode.BadRequest;
                errorCode = "ERR_BAD_REQUEST";
                message = _messageService.GetMessage(notFoundEx.Message) ?? notFoundEx.Message;
                break;
            
            case FluentValidation.ValidationException valEx:
                statusCode = (int)HttpStatusCode.BadRequest;
                errorCode = "ERR_VALIDATION_FAILED";
                message = "con cac";
                break;
            
            default:
                statusCode = (int)HttpStatusCode.InternalServerError;
                errorCode = "ERR_INTERNAL_SERVER";
                message = _messageService.GetMessage(errorCode) ?? "Lỗi";
                break;
        }

        context.Response.StatusCode = statusCode;

        var result = new
        {
            code = errorCode,
            message,
            time = DateTime.UtcNow.ToString("o") // ISO 8601 format: 2025-10-15T03:15:22.8008983Z
        };

        await context.Response.WriteAsJsonAsync(result);
    }
}
