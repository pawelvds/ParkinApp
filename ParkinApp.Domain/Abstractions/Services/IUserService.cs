using ParkinApp.DTOs;
using System.Threading.Tasks;

namespace ParkinApp.Domain.Abstractions.Services
{
    public interface IUserService
    {
        Task<UserDto> RegisterAsync(RegisterDto registerDto);
        Task<UserDto> LoginAsync(LoginDto loginDto);
    }
}