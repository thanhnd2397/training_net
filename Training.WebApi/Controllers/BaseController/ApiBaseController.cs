namespace Training.WebApi.Controllers.BaseController;

/// <summary>
/// Controller for setting the Mediator.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public abstract class ApiBaseController : ControllerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiBaseController"/> class.
    /// </summary>
    public ApiBaseController()
    {
    }
}