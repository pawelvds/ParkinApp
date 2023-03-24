using Microsoft.EntityFrameworkCore;
using ParkinApp.Domain.Abstractions.Repositories;
using ParkingApp.Entities;
using System.Threading.Tasks;
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
            return await _context.Users.AnyAsync(u => u.Login == username);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Login == username);
        }
    }
}