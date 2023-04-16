using ParkinApp.Domain.Entities;

namespace ParkinApp.Domain.Abstractions.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
        string CreateRefreshToken();
        Task InvalidateTokenAsync(string refreshToken);
        Task<int?> GetRefreshTokenAsync(string userLogin);
    }
}