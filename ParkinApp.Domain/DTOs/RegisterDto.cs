using System.ComponentModel.DataAnnotations;

namespace ParkinApp.Domain.DTOs
{
    public record RegisterDto
    {
        public string Username { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }

}