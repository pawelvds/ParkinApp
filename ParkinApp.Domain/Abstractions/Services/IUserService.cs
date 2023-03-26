using ParkinApp.Domain.Common;
using ParkinApp.DTOs;
using System.Threading.Tasks;
using ParkinApp.Domain.DTOs;

namespace ParkinApp.Domain.Abstractions.Services
{
    public interface IUserService
    {
        Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto);
        Task<Result<UserDto>> LoginAsync(LoginDto loginDto);
    }
}