using MvdBackend.Data;
using MvdBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace MvdBackend.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Employee)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetWithDetailsAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Employee)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }
    }
}