using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Training.Application.Common;
using Training.Application.Common.Interfaces;

namespace Training.WebApi.Filter
{
    public class JwtTokenFilter
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtTokenFilter(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // ✅ Lấy IMessageService từ RequestServices trong scope hiện tại
            var path = context.Request.Path.Value?.ToLower();
            if (path == "/api/auth/login") // <-- chỉnh lại theo route thực tế của bạn
            {
                await _next(context);
                return;
            }
            var messageService = context.RequestServices.GetRequiredService<IMessageService>();

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                await WriteErrorAsync(context, messageService, HttpStatusCode.Unauthorized, "ERR_AUTH_REQUIRED");
                return;
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

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
                await WriteErrorAsync(context, messageService, HttpStatusCode.Unauthorized, "ERR_AUTH_INVALID");
                return;
            }

            await _next(context);
        }

        private static async Task WriteErrorAsync(HttpContext context, IMessageService messageService, HttpStatusCode statusCode, string errorCode)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var message = messageService.GetMessage(errorCode);
            var result = new
            {
                code = errorCode,
                message = message ?? "Unauthorized"
            };

            await context.Response.WriteAsJsonAsync(result);
        }
    }
}
