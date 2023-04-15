using System.Security.Cryptography;
using System.Text;
using ParkinApp.Domain.Abstractions.Repositories;
using ParkinApp.Domain.Abstractions.Services;
using ParkinApp.Domain.Common;
using ParkinApp.Domain.DTOs;
using ParkinApp.Domain.Entities;

namespace ParkinApp.Services
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

        public async Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto)
        {
            if (await _userRepository.UserExists(registerDto.Username))
                return Result<UserDto>.Failure(new List<string> { "Username is taken" });

            using var hmac = new HMACSHA512();

            var user = new User
            {
                Login = registerDto.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            await _userRepository.AddAsync(user);

            var accessToken = _tokenService.CreateToken(user);
            var refreshToken = _tokenService.CreateRefreshToken();
            await _tokenService.StoreRefreshTokenAsync(user, refreshToken);

            return Result<UserDto>.Success(new UserDto(
                user.Login,
                accessToken,
                refreshToken
            ));
        }

        public async Task<Result<UserDto>> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByUsernameAsync((loginDto.Username));

            if (user == null)
                return Result<UserDto>.Failure(new List<string> { "Invalid username or password" });

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    return Result<UserDto>.Failure(new List<string> { "Invalid username or password" });
            }

            var accessToken = _tokenService.CreateToken(user);
            var refreshToken = _tokenService.CreateRefreshToken();
            await _tokenService.StoreRefreshTokenAsync(user, refreshToken);

            return Result<UserDto>.Success(new UserDto(
                user.Login,
                accessToken,
                refreshToken
            ));
        }

        public async Task<Result<UserDto>> LogoutAsync(string refreshToken)
        {
            var user = await _userRepository.GetUserByRefreshToken(refreshToken);

            if (user == null)
            {
                return Result<UserDto>.Failure(new List<string> { "Invalid refresh token" });
            }

            await _tokenService.InvalidateTokenAsync(refreshToken);
            return Result<UserDto>.Success(null);
        }

        public async Task<Result<UserDto>> RefreshTokenAsync(string refreshToken)
        {
            var user = await _userRepository.GetUserByRefreshToken(refreshToken);
            Console.WriteLine($"User from database: {user?.Login}, RefreshToken: {user?.RefreshToken}, RefreshTokenExpiryDate: {user?.RefreshTokenExpiryDate}");

            if (user == null)
            {
                Console.WriteLine("Invalid refresh token");
                return Result<UserDto>.Failure(new List<string> { "Invalid refresh token" });
            }

            if (user.RefreshTokenExpiryDate < DateTimeOffset.Now)
            {
                Console.WriteLine("Expired refresh token");
                return Result<UserDto>.Failure(new List<string> { "Invalid or expired refresh token" });
            }

            var accessToken = _tokenService.CreateToken(user);
            var newRefreshToken = _tokenService.CreateRefreshToken();
            await _tokenService.StoreRefreshTokenAsync(user, newRefreshToken);

            return Result<UserDto>.Success(new UserDto(
                user.Login,
                accessToken,
                newRefreshToken
            ));
        }

    }
}
