using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog;
using Training.Application.Common;

namespace Training.WebApi.Filter
{
    /// <summary>
    /// Middleware để đọc JWT token từ Header và trích xuất thông tin user.
    /// </summary>
    public class JwtTokenFilter
    {
        private readonly RequestDelegate _next;
        private readonly IMessageService _messageService;

        public JwtTokenFilter(RequestDelegate next, IMessageService messageService)
        {
            _next = next;
            _messageService = messageService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                await WriteErrorAsync(context, StatusCodes.Status401Unauthorized, "ERR_AUTH_REQUIRED");
                return;
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // Ví dụ lấy UserId và Role
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
                var role = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

                if (!string.IsNullOrEmpty(userId))
                    context.Items["UserId"] = userId;

                if (!string.IsNullOrEmpty(role))
                    context.Items["UserRole"] = role;
            }
            catch (System.Exception ex)
            {
                Log.Warning("Invalid JWT token: {Message}", ex.Message);
                await WriteErrorAsync(context, StatusCodes.Status401Unauthorized, "ERR_AUTH_INVALID");
                return;
            }

            await _next(context);
        }

        private async Task WriteErrorAsync(HttpContext context, int statusCode, string errorCode)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var message = _messageService.GetMessage(errorCode);

            var result = new
            {
                code = errorCode,
                message = message ?? "Unauthorized"
            };

            await context.Response.WriteAsJsonAsync(result);
        }
    }
}