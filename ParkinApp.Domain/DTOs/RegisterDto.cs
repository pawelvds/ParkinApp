using System.ComponentModel.DataAnnotations;

namespace ParkinApp.Domain.DTOs
{
    public record RegisterDto
    {
        [Required(ErrorMessage = "Username cannot be empty.")]
        public string Username { get; init; } = string.Empty;

        [Required(ErrorMessage = "Password cannot be empty.")]
        public string Password { get; init; } = string.Empty;
    }
}