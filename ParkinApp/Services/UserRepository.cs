using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParkinApp.Interfaces;
using ParkingApp.Entities;

namespace ParkinApp.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ParkingDbContext _context;

        public UserRepository(ParkingDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Login == username);
        }

        public async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(u => u.Login == username);
        }
    }
}