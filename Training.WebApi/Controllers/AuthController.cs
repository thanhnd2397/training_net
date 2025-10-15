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
        var result = await loginUseCase.LoginAsync(request);
        return Ok(result);
    }
}