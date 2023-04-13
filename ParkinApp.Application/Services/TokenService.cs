using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ParkinApp.Domain.Abstractions.Repositories;
using ParkinApp.Domain.Abstractions.Services;
using ParkinApp.Domain.Entities;

namespace ParkinApp.Services;

public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _key;
    private readonly IUserRepository _userRepository;

    public TokenService(IConfiguration config, IUserRepository userRepository)
    {
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"] ?? throw new InvalidOperationException()));
        _userRepository = userRepository;
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
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public string CreateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public async Task StoreRefreshTokenAsync(User user, string refreshToken)
    {
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryDate = DateTimeOffset.UtcNow.AddHours(1);
        await _userRepository.UpdateAsync(user);
    }


    public async Task InvalidateTokenAsync(string refreshToken)
    {
        var user = await _userRepository.GetUserByRefreshToken(refreshToken);
        if (user != null)
        {
            user.RefreshToken = string.Empty;
            user.RefreshTokenExpiryDate = DateTimeOffset.MinValue;
            await _userRepository.UpdateAsync(user);
        }
    }
}