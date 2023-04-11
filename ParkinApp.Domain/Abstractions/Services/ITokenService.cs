using ParkinApp.Domain.Entities;
using System.Threading.Tasks;

namespace ParkinApp.Domain.Abstractions.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
        string CreateRefreshToken();
        Task StoreRefreshTokenAsync(User user, string refreshToken);
        Task InvalidateTokenAsync(string refreshToken);
    }
}