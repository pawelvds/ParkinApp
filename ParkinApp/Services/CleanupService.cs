using Microsoft.EntityFrameworkCore;
using ParkinApp.Data;

public class CleanupService : IHostedService, IDisposable
{
    private Timer _timer;
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

    private void Cleanup(object state)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ParkingDbContext>();

        var utcNow = DateTime.UtcNow;
        var parkingSpots = context.ParkingSpots.Include(ps => ps.User).Where(ps => ps.IsReserved && ps.ReservationEndTime != null && ps.SpotTimeZoneId != null).ToList();

        foreach (var spot in parkingSpots)
        {
            try
            {
                var spotTimeZone = TimeZoneInfo.FindSystemTimeZoneById(spot.SpotTimeZoneId);
                var spotEndTimeInUtc = TimeZoneInfo.ConvertTimeToUtc(spot.ReservationEndTime.Value, spotTimeZone);

                if (spotEndTimeInUtc <= utcNow)
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing parking spot cleanup: {ex.Message}");
            }
        }

        context.SaveChanges();
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