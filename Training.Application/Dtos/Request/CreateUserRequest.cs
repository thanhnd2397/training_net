namespace Training.Application.Dtos.Request;

public class CreateUserRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int? Age { get; set; } = null;
    public string Address { get; set; } = string.Empty;
    public string Mail { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}