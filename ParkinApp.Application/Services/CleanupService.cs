using Microsoft.EntityFrameworkCore;
using ParkinApp.Domain.Entities;
using ParkinApp.Persistence.Data;

namespace ParkinApp.Services;

public class CleanupService : IHostedService, IDisposable
{
    private Timer? _timer;
    private readonly IServiceScopeFactory _scopeFactory;

    public CleanupService(IServiceScopeFactory scopeFactory)
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

        var utcNow = DateTime.UtcNow;
        var parkingSpots = context.ParkingSpots.Include(ps => ps.User)
                                                              .Where(ps => ps.IsReserved && ps.ReservationEndTime != null && ps.SpotTimeZoneId != null)
                                                              .ToList();

        foreach (var spot in parkingSpots)
        {
            try
            {
                if (spot.ReservationEndTime.HasValue && !string.IsNullOrEmpty(spot.SpotTimeZoneId))
                {
                    var spotTimeZone = TimeZoneInfo.FindSystemTimeZoneById(spot.SpotTimeZoneId);
                    var spotEndTimeInLocal = TimeZoneInfo.ConvertTimeToUtc(spot.ReservationEndTime.Value, spotTimeZone);

                    if (spotEndTimeInLocal <= utcNow)
                    {
                        CleanupReservation(spot);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing parking spot cleanup: {ex.Message}"); //throw new exception
            }
        }

        context.SaveChanges();
    }

    private void CleanupReservation(ParkingSpot spot)
    {
        spot.IsReserved = false;
        spot.UserId = null;
        spot.ReservationTime = null;
        spot.ReservationEndTime = null;

        if (spot.User != null)
        {
            spot.User.ReservedSpotId = null;
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
