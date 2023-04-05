using ParkinApp.Domain.Abstractions.Repositories;
using ParkinApp.Domain.Entities;
using ParkinApp.Persistence.Data;

namespace ParkinApp.Persistence.Repositories
{
    public class ParkingSpotRepository : GenericRepository<ParkingSpot>, IParkingSpotRepository
    {
        private readonly ParkingDbContext _context;

        public ParkingSpotRepository(ParkingDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ParkingSpot?> GetParkingSpotByIdAsync(int id)
        {
            return await _context.ParkingSpots.FindAsync(id);
        }
    }
}