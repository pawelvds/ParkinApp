using System.Threading.Tasks;
using ParkinApp.Domain.Common;
using ParkinApp.Domain.DTOs;

namespace ParkinApp.Domain.Abstractions.Services
{
    public interface IUserService
    {
        Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto);
        Task<Result<UserDto>> LoginAsync(LoginDto loginDto);
        Task<Result<UserDto>> LogoutAsync(string refreshToken);
        Task<Result<UserDto>> RefreshTokenAsync(string refreshToken);
    }
}