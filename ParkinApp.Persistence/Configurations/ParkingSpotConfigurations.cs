using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkinApp.Domain.Entities;

namespace ParkinApp.Persistence.Configurations;

    public class ParkingSpotConfiguration : IEntityTypeConfiguration<ParkingSpot>
    {
        private const string DefaultTimeZone = "Europe/Warsaw";
        
        public void Configure(EntityTypeBuilder<ParkingSpot> builder)
        {
            builder.HasKey(ps => ps.Id);

            builder.HasOne(ps => ps.User)
                .WithOne(u => u.ReservedSpot)
                .HasForeignKey<ParkingSpot>(ps => ps.UserId);

            var defaultTimeZone = DefaultTimeZone;

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