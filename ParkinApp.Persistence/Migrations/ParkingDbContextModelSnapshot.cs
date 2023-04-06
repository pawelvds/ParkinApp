﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ParkinApp.Persistence.Data;

#nullable disable

namespace ParkinApp.Persistence.Migrations
{
    [DbContext(typeof(ParkingDbContext))]
    partial class ParkingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ParkinApp.Domain.Entities.ParkingSpot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("SpotTimeZone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ParkingSpots");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            SpotTimeZone = "Europe/Warsaw"
                        },
                        new
                        {
                            Id = 2,
                            SpotTimeZone = "Europe/Warsaw"
                        },
                        new
                        {
                            Id = 3,
                            SpotTimeZone = "Europe/Warsaw"
                        },
                        new
                        {
                            Id = 4,
                            SpotTimeZone = "Europe/Warsaw"
                        },
                        new
                        {
                            Id = 5,
                            SpotTimeZone = "Europe/Warsaw"
                        },
                        new
                        {
                            Id = 6,
                            SpotTimeZone = "Europe/Warsaw"
                        },
                        new
                        {
                            Id = 7,
                            SpotTimeZone = "Europe/Warsaw"
                        },
                        new
                        {
                            Id = 8,
                            SpotTimeZone = "Europe/Warsaw"
                        },
                        new
                        {
                            Id = 9,
                            SpotTimeZone = "Europe/Warsaw"
                        },
                        new
                        {
                            Id = 10,
                            SpotTimeZone = "Europe/Warsaw"
                        },
                        new
                        {
                            Id = 11,
                            SpotTimeZone = "Europe/Warsaw"
                        },
                        new
                        {
                            Id = 12,
                            SpotTimeZone = "Europe/Warsaw"
                        },
                        new
                        {
                            Id = 13,
                            SpotTimeZone = "Europe/Warsaw"
                        },
                        new
                        {
                            Id = 14,
                            SpotTimeZone = "Europe/Warsaw"
                        },
                        new
                        {
                            Id = 15,
                            SpotTimeZone = "Europe/Warsaw"
                        },
                        new
                        {
                            Id = 16,
                            SpotTimeZone = "Europe/Warsaw"
                        },
                        new
                        {
                            Id = 17,
                            SpotTimeZone = "Europe/Warsaw"
                        },
                        new
                        {
                            Id = 18,
                            SpotTimeZone = "Europe/Warsaw"
                        },
                        new
                        {
                            Id = 19,
                            SpotTimeZone = "Europe/Warsaw"
                        },
                        new
                        {
                            Id = 20,
                            SpotTimeZone = "Europe/Warsaw"
                        });
                });

            modelBuilder.Entity("ParkinApp.Domain.Entities.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedReservationTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("ParkingSpotId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReservationEndTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParkingSpotId");

                    b.HasIndex("UserId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("ParkinApp.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("UserTimeZone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ParkinApp.Domain.Entities.Reservation", b =>
                {
                    b.HasOne("ParkinApp.Domain.Entities.ParkingSpot", "ParkingSpot")
                        .WithMany("Reservations")
                        .HasForeignKey("ParkingSpotId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ParkinApp.Domain.Entities.User", "User")
                        .WithMany("Reservations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ParkingSpot");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ParkinApp.Domain.Entities.ParkingSpot", b =>
                {
                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("ParkinApp.Domain.Entities.User", b =>
                {
                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}
