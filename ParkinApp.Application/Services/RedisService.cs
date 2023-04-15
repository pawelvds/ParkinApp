using ParkinApp.Domain.Abstractions.Repositories;
using ParkinApp.Domain.Abstractions.Services;
using StackExchange.Redis;

public class RedisService : IRedisService
{
    private readonly IConnectionMultiplexer _redis;

    public RedisService(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task SetRefreshTokenAsync(string refreshToken, int userId, TimeSpan expiresIn)
    {
        var db = _redis.GetDatabase();
        await db.StringSetAsync(refreshToken, userId, expiresIn);
    }

    public async Task<int?> GetUserIdByRefreshTokenAsync(string refreshToken)
    {
        var db = _redis.GetDatabase();
        var userId = await db.StringGetAsync(refreshToken);

        if (userId.IsNullOrEmpty)
        {
            return null;
        }

        return (int)userId;
    }



    public async Task RemoveRefreshTokenAsync(string refreshToken)
    {
        var db = _redis.GetDatabase();
        await db.KeyDeleteAsync(refreshToken);
    }
}