using Microsoft.EntityFrameworkCore;
using Training.Application.IRepositories;
using Training.Domain.Entities;
using Training.Infrastructure.Persistence;

namespace Training.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.UserName == username);
    }

}