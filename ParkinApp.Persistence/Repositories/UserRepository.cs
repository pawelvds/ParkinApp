using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParkinApp.Domain.Abstractions.Repositories;
using ParkinApp.Persistence.Data;
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
            return await _context.Users.FirstOrDefaultAsync(u => u.Login == username) ??
                   throw new ArgumentException("x");
        }

        public async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(u => u.Login == username);
        }

        public Task<User> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}