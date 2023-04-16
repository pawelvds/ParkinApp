namespace ParkinApp.Domain.Abstractions.Services
{
    public interface IRedisService
    {
        Task SetRefreshTokenAsync(string refreshToken, int userId, TimeSpan expiresIn);
        Task<int?> GetUserIdByRefreshTokenAsync(string refreshToken);
        Task RemoveRefreshTokenAsync(string refreshToken);
    }
}