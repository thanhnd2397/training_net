namespace Training.WebApi.Controllers;

public class UserController : ApiBaseController
{
    /// <summary>
    /// Đăng ký người dùng
    /// </summary>
    [HttpPost]
    public Task<IActionResult> CreateUser([FromBody] CreateUserRequest? request)
    { 
        
    }
    
}