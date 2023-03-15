using ParkingApp.Entities;

namespace ParkinApp.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
}