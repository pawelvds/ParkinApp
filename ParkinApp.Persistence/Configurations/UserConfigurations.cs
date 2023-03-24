using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkingApp.Entities;

namespace ParkinApp.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Login).IsRequired().HasMaxLength(30);
            builder.HasIndex(u => u.Login).IsUnique();

            builder.HasOne(u => u.ReservedSpot)
                .WithOne(ps => ps.User)
                .HasForeignKey<User>(u => u.ReservedSpotId);
        }
    }
}