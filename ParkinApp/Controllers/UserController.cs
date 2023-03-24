using Microsoft.AspNetCore.Mvc;
using ParkinApp.DTOs;
using ParkingApp.Entities;
using System.Security.Cryptography;
using System.Text;
using ParkinApp.Domain.Abstractions.Repositories;
using ParkinApp.Domain.Abstractions.Services;
using FluentValidation;

namespace ParkinApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IValidator<LoginDto> _loginValidator;

        public UserController(IUserRepository userRepository, ITokenService tokenService,
            IValidator<RegisterDto> registerValidator, IValidator<LoginDto> loginValidator)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var validationResult = await _registerValidator.ValidateAsync(registerDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (await _userRepository.UserExists(registerDto.Username)) return BadRequest("Username is taken");

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

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var validationResult = await _loginValidator.ValidateAsync(loginDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var user = await _userRepository.GetUserByUsername(loginDto.Username);

            if (user == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            }

            // Aktualizacja strefy czasowej użytkownika podczas logowania
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
