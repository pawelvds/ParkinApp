using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkingApp.Entities;

namespace ParkingApp.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasOne(u => u.ReservedSpot)
                .WithOne(ps => ps.User)
                .HasForeignKey<User>(u => u.ReservedSpotId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}