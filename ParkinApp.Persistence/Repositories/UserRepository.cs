using Microsoft.EntityFrameworkCore;
using ParkinApp.Domain.Abstractions.Repositories;
using ParkinApp.Domain.Abstractions.Services;
using ParkinApp.Domain.Entities;
using ParkinApp.Persistence.Data;

namespace ParkinApp.Persistence.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ParkingDbContext _context;
        private readonly IRedisService _redisService;

        public UserRepository(ParkingDbContext context, IRedisService redisService) : base(context)
        {
            _context = context;
            _redisService = redisService;
        }

        public async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(u => u.Login.Equals(username));
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Login.Equals(username));
        }

        public async Task<User?> GetUserByRefreshToken(string refreshToken)
        {
            var userId = await _redisService.GetUserIdByRefreshTokenAsync(refreshToken);

            if (userId.HasValue)
            {
                return await _context.Set<User>()
                    .Where(u => u.Id == userId.Value)
                    .FirstOrDefaultAsync();
            }
            return null;
        }



    }
}