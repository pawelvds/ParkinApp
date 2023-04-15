using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ParkinApp.Domain.Abstractions.Repositories;
using ParkinApp.Domain.Abstractions.Services;
using ParkinApp.Domain.Entities;

namespace ParkinApp.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly IUserRepository _userRepository;
        private readonly IRedisService _redisService;

        public TokenService(IConfiguration config, IUserRepository userRepository, IRedisService redisService)
        {
            _key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["TokenKey"] ?? throw new InvalidOperationException()));
            _userRepository = userRepository;
            _redisService = redisService;
        }

        public string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Login)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var refreshToken = Convert.ToBase64String(randomNumber);

            return refreshToken;
        }

        public async Task StoreRefreshTokenAsync(User user, string refreshToken)
        {
            user.RefreshTokenExpiryDate = DateTimeOffset.UtcNow.AddHours(1);
            await _redisService.SetRefreshTokenAsync(refreshToken, user.Id,
                user.RefreshTokenExpiryDate - DateTimeOffset.UtcNow);

            Console.WriteLine($"Stored refresh token: {refreshToken} for user {user.Login} in cache");

            await _userRepository.UpdateAsync(user);
        }

        public async Task InvalidateTokenAsync(string refreshToken)
        {
            await _redisService.RemoveRefreshTokenAsync(refreshToken);
        }

        public async Task<int?> GetRefreshTokenAsync(string userLogin)
        {
            var user = await _userRepository.GetUserByUsernameAsync(userLogin);
            if (user == null) return null;

            var refreshToken = await _redisService.GetUserIdByRefreshTokenAsync(user.Id.ToString());

            Console.WriteLine($"Retrieved refresh token: {refreshToken} for user {userLogin} from cache");

            return refreshToken;
        }
    }
}
