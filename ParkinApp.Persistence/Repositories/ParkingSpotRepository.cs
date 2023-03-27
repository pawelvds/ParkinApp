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

        public async Task ReserveSpotForUserAsync(ParkingSpot parkingSpot, User user)
        {
            parkingSpot.IsReserved = true;
            parkingSpot.UserId = user.Id;
            user.ReservedSpotId = parkingSpot.Id;

            _context.ParkingSpots.Update(parkingSpot);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task CancelReservationAsync(ParkingSpot parkingSpot)
        {
            parkingSpot.IsReserved = false;
            parkingSpot.UserId = null;

            _context.ParkingSpots.Update(parkingSpot);
            await _context.SaveChangesAsync();
        }
    }
}