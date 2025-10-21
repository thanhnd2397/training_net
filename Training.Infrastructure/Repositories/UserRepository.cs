namespace Training.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.UserName == username);
    }
    
    public async Task<int?> AddAsync(User user)
    {
        await context.Users.AddAsync(user);

        return user.Id;
    }
    
    public async Task<bool> ExistsByUsernameAsync(string username)
    {
        return await context.Users
            .AnyAsync(u => u.UserName == username && !u.DeleteFlg);
    }

}