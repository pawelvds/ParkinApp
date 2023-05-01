using System.Text.Json;
using System.Text.Json.Serialization;
using ParkinApp.Domain.Abstractions.Services;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

namespace ParkinApp.Services;

public class RedisService : IRedisService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly ILoggerFactory _loggerFactory;

    public RedisService(IConnectionMultiplexer redis, ILoggerFactory loggerFactory)
    {
        _redis = redis;
        _loggerFactory = loggerFactory;
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
    
    public async Task SetAsync<T>(string key, T value, TimeSpan? expiresIn = null)
    {
        var db = _redis.GetDatabase();
        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve
        };
        await db.StringSetAsync(key, JsonSerializer.Serialize(value, options), expiresIn);
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var db = _redis.GetDatabase();
        var data = await db.StringGetAsync(key);
        if (data.IsNullOrEmpty) return default;

        return JsonSerializer.Deserialize<T>(data.ToString());
    }

    public async Task RemoveAsync(string key)
    {
        var db = _redis.GetDatabase();
        await db.KeyDeleteAsync(key);
    }
    
    public async Task<IRedLock> CreateLockAsync(string resource, TimeSpan expiryTime)
    {
        var endPoints = _redis.GetEndPoints();
        var redLockEndPoints = new List<RedLockEndPoint>();

        foreach (var endPoint in endPoints)
        {
            redLockEndPoints.Add(new RedLockEndPoint(endPoint));
        }

        var redLockConfiguration = new RedLockConfiguration(redLockEndPoints);
        var redLockFactory = RedLockFactory.Create(redLockEndPoints, _loggerFactory);
        return await redLockFactory.CreateLockAsync(resource, expiryTime);
    }

}