using ParkinApp.Domain.Entities;

namespace ParkinApp.Domain.Abstractions.Repositories
{
    public interface IParkingSpotRepository : IGenericRepository<ParkingSpot>
    {
        Task<ParkingSpot?> GetParkingSpotByIdAsync(int id);
        
        IQueryable<ParkingSpot> GetQueryable();
        
        Task<List<ParkingSpot>> GetAllParkingSpotsWithReservationsAsync();

    }
}