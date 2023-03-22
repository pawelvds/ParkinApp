using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkingApp.Entities;

namespace ParkingApp.Configurations
{
    public class ParkingSpotConfiguration : IEntityTypeConfiguration<ParkingSpot>
    {
        public void Configure(EntityTypeBuilder<ParkingSpot> builder)
        {
            builder.HasKey(ps => ps.Id);

            builder.HasOne(ps => ps.User)
                .WithOne(u => u.ReservedSpot)
                .HasForeignKey<ParkingSpot>(ps => ps.UserId);

            string defaultTimeZone = "Europe/Warsaw";

            builder.HasData(
                Enumerable.Range(1, 10)
                    .Select(i => new ParkingSpot
                    {
                        Id = i,
                        IsReserved = false,
                        ReservationTime = null,
                        UserId = null,
                        SpotTimeZoneId = defaultTimeZone,
                        UserTimeZoneId = defaultTimeZone 
                    }));
        }
    }
}