namespace ParkinApp.Domain.DTOs;

public class OccupiedParkingSpotDto
{
    public int ParkingSpotId { get; }
    public string Login { get; }

    public OccupiedParkingSpotDto(int parkingSpotId, string login)
    {
        ParkingSpotId = parkingSpotId;
        Login = login;
    }
}