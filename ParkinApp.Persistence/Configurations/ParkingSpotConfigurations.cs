using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkinApp.Domain.Entities;
using System.Linq;

namespace ParkinApp.Persistence.Configurations
{
    public class ParkingSpotConfiguration : IEntityTypeConfiguration<ParkingSpot>
    {
        private const string DefaultTimeZone = "Europe/Warsaw";

        public void Configure(EntityTypeBuilder<ParkingSpot> builder)
        {
            builder.HasKey(ps => ps.Id);

            builder.Property(ps => ps.SpotTimeZone).IsRequired();

            SeedData(builder);
        }

        private void SeedData(EntityTypeBuilder<ParkingSpot> builder)
        {
            var defaultTimeZone = DefaultTimeZone;

            builder.HasData(
                Enumerable.Range(1, 20)
                    .Select(i => new ParkingSpot
                    {
                        Id = i,
                        SpotTimeZone = defaultTimeZone
                    }));
        }
    }
}