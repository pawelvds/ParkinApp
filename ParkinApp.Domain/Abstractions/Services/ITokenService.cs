using ParkingApp.Entities;

namespace ParkinApp.Domain.Abstractions.Services;

public interface ITokenService
{
    string CreateToken(User user);
}