namespace Training.Application.IRepositories;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);

    Task<int?> AddAsync(User user);
    
    Task<bool> ExistsByUsernameAsync(string username);
}