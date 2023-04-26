using Microsoft.Extensions.Caching.Memory;
using ParkinApp.Domain.Abstractions.Repositories;
using ParkinApp.Domain.Abstractions.Services;
using ParkinApp.Domain.Entities;

namespace ParkinApp.Application.Services
{
    public class ParkingSpotCacheService : IParkingSpotCacheService
    {
        private readonly IMemoryCache _cache;
        private const string ParkingSpotCacheKeyPrefix = "ParkingSpot_";

        public ParkingSpotCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<ParkingSpot?> GetParkingSpotByIdAsync(int parkingSpotId, IParkingSpotRepository parkingSpotRepository)
        {
            if (!_cache.TryGetValue(GetParkingSpotCacheKey(parkingSpotId), out ParkingSpot? parkingSpot))
            {
                parkingSpot = await parkingSpotRepository.GetParkingSpotByIdAsync(parkingSpotId);
                _cache.Set(GetParkingSpotCacheKey(parkingSpotId), parkingSpot);
            }

            return parkingSpot;
        }

        public async Task RefreshParkingSpotCacheAsync(int parkingSpotId, IParkingSpotRepository parkingSpotRepository)
        {
            var parkingSpot = await parkingSpotRepository.GetParkingSpotByIdAsync(parkingSpotId);
            UpdateParkingSpotCache(parkingSpot);
        }

        public void UpdateParkingSpotCache(ParkingSpot? parkingSpot)
        {
            if (parkingSpot != null) _cache.Set(GetParkingSpotCacheKey(parkingSpot.Id), parkingSpot);
        }

        private string GetParkingSpotCacheKey(int parkingSpotId)
        {
            return $"{ParkingSpotCacheKeyPrefix}{parkingSpotId}";
        }
    }
}