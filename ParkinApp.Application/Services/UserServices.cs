using System.Threading.Tasks;
using ParkinApp.Domain.Abstractions.Repositories;
using ParkingApp.Entities;
using ParkinApp.Domain.Abstractions.Services;
using System.Security.Cryptography;
using System.Text;
using ParkinApp.DTOs;

namespace ParkinApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            if (await _userRepository.UserExists(registerDto.Username))
                throw new ArgumentException("Username is taken");

            using var hmac = new HMACSHA512();

            var user = new User
            {
                Login = registerDto.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key,
                UserTimeZoneId = "UTC"
            };

            await _userRepository.AddAsync(user);

            return new UserDto
            {
                Username = user.Login,
                Token = _tokenService.CreateToken(user),
            };
        }

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByUsername(loginDto.Username);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    throw new UnauthorizedAccessException("Invalid password");
            }

            // Update the user's time zone during login
            user.UserTimeZoneId = loginDto.UserTimeZoneId;
            await _userRepository.UpdateAsync(user);

            return new UserDto
            {
                Username = user.Login,
                Token = _tokenService.CreateToken(user),
                UserTimeZoneId = user.UserTimeZoneId
            };
        }
    }
}
