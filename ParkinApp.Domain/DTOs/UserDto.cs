namespace ParkinApp.Domain.DTOs
{
    public record UserDto(string Username, string AccessToken, string RefreshToken);
}