using ParkingApp.Entities;
using Microsoft.EntityFrameworkCore;


namespace ParkingApp.Data;

public class ParkingDbContext : DbContext
{
    
    public ParkingDbContext(DbContextOptions<ParkingDbContext> options) : base(options)
    {
    }

    public DbSet<ParkingSpot> ParkingSpots { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ParkingSpot>()
            .HasKey(ps => ps.Id);

        // Initialization 10 spots
        modelBuilder.Entity<ParkingSpot>()
            .HasData(
                Enumerable.Range(1, 10)
                    .Select(i => new ParkingSpot
                    {
                        Id = i,
                        IsReserved = false,
                        ReservationTime = null,
                        TimeZoneId = "UTC+1"
                    }));
    }
}
