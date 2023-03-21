using System.ComponentModel.DataAnnotations;

namespace ParkinApp.DTOs;

public class LoginDto
{
    public string Username { get; set; }
    public string Password { get; set; }
    [Required]
    public string UserTimeZoneId { get; set; } 
}