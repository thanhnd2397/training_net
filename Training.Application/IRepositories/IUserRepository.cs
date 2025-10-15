namespace Training.Application.IRepositories;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
}