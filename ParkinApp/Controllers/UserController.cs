using Microsoft.AspNetCore.Mvc;
using ParkinApp.DTOs;
using ParkinApp.Domain.Abstractions.Services;


namespace ParkinApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await _userService.RegisterAsync(registerDto);
            if (result.IsSuccessful)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await _userService.LoginAsync(loginDto);
            if (result.IsSuccessful)
            {
                return Ok(result.Value);
            }

            return Unauthorized(result.Errors);
        }
    }
}