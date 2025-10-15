using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Training.Application.Common.Exceptions;
using Training.Application.Common.Models;
using Training.Application.Dtos.Request;
using Training.Application.IUseCases;
using Training.WebApi.Controllers.BaseController;

namespace Training.WebApi.Controllers;

public class AuthController(ILoginUseCase loginUseCase) : ApiBaseController
{
    /// <summary>
    /// Đăng nhập người dùng
    /// </summary>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest? request, [FromServices] IValidator<LoginRequest> validator)
    {
        var result = await loginUseCase.ExecuteAsync(request);
        return Ok(result);
    }
}