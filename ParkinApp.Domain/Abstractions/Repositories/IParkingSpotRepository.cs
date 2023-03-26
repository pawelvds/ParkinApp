using ParkinApp.Domain.Entities;
using ParkingApp.Entities;

namespace ParkinApp.Domain.Abstractions.Repositories
{
    public interface IParkingSpotRepository : IGenericRepository<ParkingSpot>
    {
        Task<ParkingSpot?> GetParkingSpotByIdAsync(int id);
        Task ReserveSpotForUserAsync(ParkingSpot parkingSpot, User user);
        Task CancelReservationAsync(ParkingSpot parkingSpot);
    }
}