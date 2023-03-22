using Microsoft.AspNetCore.Mvc;
using ParkinApp.DTOs;
using ParkinApp.Interfaces;
using ParkingApp.Entities;
using ParkinApp.Data;
using System.Security.Cryptography;
using System.Text;

namespace ParkinApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly ParkingDbContext _context;

        public UserController(IUserRepository userRepository, ITokenService tokenService, ParkingDbContext context)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _userRepository.UserExists(registerDto.Username)) return BadRequest("Username is taken");

            using var hmac = new HMACSHA512();

            var user = new User
            {
                Login = registerDto.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key,
                UserTimeZoneId = "UTC"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Username = user.Login,
                Token = _tokenService.CreateToken(user),
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByUsername(loginDto.Username);

            if (user == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            }

            // Walidacja UserTimeZoneId
            if (string.IsNullOrEmpty(loginDto.UserTimeZoneId))
            {
                return BadRequest("User time zone ID is required.");
            }

            try
            {
                TimeZoneInfo.FindSystemTimeZoneById(loginDto.UserTimeZoneId);
            }
            catch (TimeZoneNotFoundException)
            {
                return BadRequest("Invalid time zone ID.");
            }

            // Aktualizacja strefy czasowej użytkownika podczas logowania
            user.UserTimeZoneId = loginDto.UserTimeZoneId;
            _context.Update(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Username = user.Login,
                Token = _tokenService.CreateToken(user),
                UserTimeZoneId = user.UserTimeZoneId
            };
        }
    }
}
