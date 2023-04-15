using Microsoft.AspNetCore.Authorization;
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
        private readonly ITokenService _tokenService;

        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
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
        
        [AllowAnonymous]
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
        
        [AllowAnonymous]
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
        
        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(LogoutDto logoutDto)
        {
            Console.WriteLine("Received refresh token request: " + logoutDto.RefreshToken);
            var result = await _userService.RefreshTokenAsync(logoutDto.RefreshToken);

            if (result.IsFailure || result.Value == null)
            {
                Console.WriteLine("Refresh token request failed: " + string.Join(", ", result.Errors));
                return BadRequest(result.Errors);
            }

            var userDto = result.Value;
            Console.WriteLine("Refresh token request succeeded.");
            return Ok(new
            {
                accessToken = userDto.AccessToken,
                refreshToken = userDto.RefreshToken
            });
        }

        [AllowAnonymous]
        [HttpGet("getrefreshtoken/{userLogin}")]
        public async Task<ActionResult<string>> GetRefreshToken(string userLogin)
        {
            var refreshToken = await _tokenService.GetRefreshTokenAsync(userLogin);
            if (refreshToken == null)
            {
                return NotFound("Refresh token not found.");
            }
            return Ok(refreshToken);
        }
        
    }
}
