using Microsoft.EntityFrameworkCore;
using ParkinApp.Domain.Entities;
using ParkinApp.Persistence.Data;

namespace ParkinApp.Services
{
    public class CleanupExpiredReservations : IHostedService, IDisposable
    {
        private Timer? _timer;
        private readonly IServiceScopeFactory _scopeFactory;

        public CleanupExpiredReservations(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Cleanup, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        private void Cleanup(object? state)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ParkingDbContext>();

            var utcNow = DateTimeOffset.UtcNow;
            var reservations = context.Reservations.Include(r => r.ParkingSpot)
                .Include(r => r.User)
                .Where(r => r.ReservationEndTime < utcNow) 
                .ToList();

            foreach (var reservation in reservations)
            {
                try
                {
                    if (!string.IsNullOrEmpty(reservation.ParkingSpot?.SpotTimeZone))
                    {
                        var spotTimeZone = TimeZoneInfo.FindSystemTimeZoneById(reservation.ParkingSpot.SpotTimeZone);
                        var spotEndTimeInLocal = TimeZoneInfo.ConvertTime(reservation.ReservationEndTime, spotTimeZone);

                        if (spotEndTimeInLocal.UtcDateTime <= utcNow)
                        {
                            CleanupReservation(context, reservation);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing parking spot cleanup: {ex.Message}");
                }
            }

            context.SaveChanges();
        }


        private void CleanupReservation(ParkingDbContext context, Reservation reservation)
        {
            if (reservation.User != null && reservation.ParkingSpot != null)
            {
                context.Reservations.Remove(reservation);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
