using System;
using System.Threading.Tasks;
using RedLockNet;
using RedLockNet.SERedis;

namespace ParkinApp.Domain.Abstractions.Services
{
    public interface IRedisService
    {
        Task SetRefreshTokenAsync(string refreshToken, int userId, TimeSpan expiresIn);
        Task<int?> GetUserIdByRefreshTokenAsync(string refreshToken);
        Task RemoveRefreshTokenAsync(string refreshToken);
        
        Task SetAsync<T>(string key, T value, TimeSpan? expiresIn = null);
        Task<T> GetAsync<T>(string key);
        Task RemoveAsync(string key);
        Task<IRedLock> CreateLockAsync(string resource, TimeSpan expiryTime);
    }
}