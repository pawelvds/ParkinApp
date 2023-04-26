using ParkinApp.Domain.Abstractions.Repositories;
using ParkinApp.Domain.Entities;
using System.Threading.Tasks;

namespace ParkinApp.Domain.Abstractions.Services
{
    public interface IParkingSpotCacheService
    {
        Task<ParkingSpot?> GetParkingSpotByIdAsync(int parkingSpotId, IParkingSpotRepository parkingSpotRepository);
        Task RefreshParkingSpotCacheAsync(int parkingSpotId, IParkingSpotRepository parkingSpotRepository);
        void UpdateParkingSpotCache(ParkingSpot? parkingSpot);
    }
}