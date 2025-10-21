namespace Training.WebApi.Controllers;

public class UserController(IUserUseCase userUseCase) : ApiBaseController
{
    /// <summary>
    /// Đăng ký người dùng
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest? request)
    {
        var result = await userUseCase.CreateUser(request);
        return Ok(result);
    }
    
}