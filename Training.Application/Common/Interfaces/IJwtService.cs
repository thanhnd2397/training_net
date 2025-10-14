namespace Training.Application.Common;

public interface IJwtService
{
    string GenerateToken(string userId, string userName);
}