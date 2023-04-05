using Microsoft.EntityFrameworkCore;
using ParkinApp.Domain.Abstractions.Repositories;
using ParkinApp.Domain.Entities;
using ParkinApp.Persistence.Data;

namespace ParkinApp.Persistence.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ParkingDbContext _context;

        public UserRepository(ParkingDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(u => u.Login.Equals(username));
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Login.Equals(username));
        }
        
        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

    }
}