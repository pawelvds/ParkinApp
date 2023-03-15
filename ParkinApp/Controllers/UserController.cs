using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkinApp.DTOs;
using ParkinApp.Interfaces;
using ParkingApp.Entities;
using System.Threading.Tasks;
using ParkinApp.Data;

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

            var user = new User
            {
                Login = registerDto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Username = user.Login,
                Token = _tokenService.CreateToken(user)
            };
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByUsername(loginDto.Username);

            if (user == null) return Unauthorized("Invalid username");

            // Sprawdź hasło
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid password");
            }

            return new UserDto
            {
                Username = user.Login,
                Token = _tokenService.CreateToken(user)
            };
        }

    }
}
