using Microsoft.AspNetCore.Mvc;
using ParkinApp.Domain.Abstractions.Services;
using ParkinApp.Domain.DTOs;


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
        
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(LogoutDto logoutDto)
        {
            var result = await _userService.LogoutAsync(logoutDto.RefreshToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(LogoutDto logoutDto)
        {
            var result = await _userService.RefreshTokenAsync(logoutDto.RefreshToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Errors);
            }

            var userDto = result.Value;
            return Ok(new
            {
                accessToken = userDto.AccessToken,
                refreshToken = userDto.RefreshToken
            });
        }


    }
}