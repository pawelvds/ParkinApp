using System.Security.Cryptography;
using System.Text;
using FluentValidation;
using ParkinApp.Domain.Abstractions.Repositories;
using ParkinApp.Domain.Abstractions.Services;
using ParkinApp.Domain.Common;
using ParkinApp.DTOs;
using ParkingApp.Entities;

namespace ParkinApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IValidator<LoginDto> _loginValidator;

        public UserService(IUserRepository userRepository, ITokenService tokenService,
            IValidator<RegisterDto> registerValidator, IValidator<LoginDto> loginValidator)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }

        public async Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto)
        {
            var validationResult = await _registerValidator.ValidateAsync(registerDto);
            if (!validationResult.IsValid)
            {
                return Result<UserDto>.Failure(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            if (await _userRepository.UserExists(registerDto.Username))
                return Result<UserDto>.Failure(new List<string> { "Username is taken" });

            using var hmac = new HMACSHA512();

            var user = new User
            {
                Login = registerDto.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key,
                UserTimeZoneId = "UTC"
            };

            await _userRepository.AddAsync(user);

            return Result<UserDto>.Success(new UserDto
            {
                Username = user.Login,
                Token = _tokenService.CreateToken(user),
            });
        }

        public async Task<Result<UserDto>> LoginAsync(LoginDto loginDto)
        {
            var validationResult = await _loginValidator.ValidateAsync(loginDto);
            if (!validationResult.IsValid)
            {
                return Result<UserDto>.Failure(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var user = await _userRepository.GetUserByUsername(loginDto.Username);

            if (user == null)
                return Result<UserDto>.Failure(new List<string> { "Invalid username" });

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    return Result<UserDto>.Failure(new List<string> { "Invalid password" });
            }

            // Update the user's time zone during login
            user.UserTimeZoneId = loginDto.UserTimeZoneId;
            await _userRepository.UpdateAsync(user);

            return Result<UserDto>.Success(new UserDto
            {
                Username = user.Login,
                Token = _tokenService.CreateToken(user),
                UserTimeZoneId = user.UserTimeZoneId
            });
        }
    }
}
