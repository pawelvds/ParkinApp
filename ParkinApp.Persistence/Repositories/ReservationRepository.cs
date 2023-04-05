using Microsoft.EntityFrameworkCore;
using ParkinApp.Domain.Abstractions.Repositories;
using ParkinApp.Domain.Entities;
using ParkinApp.Persistence.Data;

namespace ParkinApp.Persistence.Repositories
{
    public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
    {
        private readonly ParkingDbContext _context;

        public ReservationRepository(ParkingDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<Reservation?> GetActiveUserReservationAsync(int userId)
        {
            return await _context.Reservations
                .Where(r => r.UserId == userId && r.ReservationEndTime > DateTime.UtcNow)
                .FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(Reservation reservation)
        {
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }
    }
}