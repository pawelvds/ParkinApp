using ParkinApp.Domain.Abstractions.Repositories;
using ParkinApp.DTOs;

namespace ParkinApp.Domain.Abstractions.Services;

public interface IParkingSpotService
{
    Task<List<ParkingSpotDto>> GetAllParkingSpotsAsync();
}

public class ParkingSpotService : IParkingSpotService
{
    private readonly IParkingSpotRepository _parkingSpotRepository;

    public ParkingSpotService(IParkingSpotRepository parkingSpotRepository)
    {
        _parkingSpotRepository = parkingSpotRepository;
    }

    public async Task<List<ParkingSpotDto>> GetAllParkingSpotsAsync()
    {
        var parkingSpots = await _parkingSpotRepository.GetAllAsync();
        return parkingSpots.Select(ps => new ParkingSpotDto
        {
            Id = ps.Id,
            IsReserved = ps.IsReserved
        }).ToList();
    }
}