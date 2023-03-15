using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkingApp.Entities;
using System.Linq;

namespace ParkingApp.Configurations
{
    public class ParkingSpotConfiguration : IEntityTypeConfiguration<ParkingSpot>
    {
        public void Configure(EntityTypeBuilder<ParkingSpot> builder)
        {
            builder.HasKey(ps => ps.Id);

            // Initialization 10 spots
            builder.HasData(
                Enumerable.Range(1, 10)
                    .Select(i => new ParkingSpot
                    {
                        Id = i,
                        IsReserved = false,
                        ReservationTime = null,
                        UserId = null,
                        TimeZoneId = "UTC+1"
                    }));
        }
    }
}