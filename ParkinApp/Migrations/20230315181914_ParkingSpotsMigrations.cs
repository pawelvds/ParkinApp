using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ParkinApp.Migrations
{
    /// <inheritdoc />
    public partial class ParkingSpotsMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParkingSpots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsReserved = table.Column<bool>(type: "INTEGER", nullable: false),
                    ReservationTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TimeZoneId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingSpots", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ParkingSpots",
                columns: new[] { "Id", "IsReserved", "ReservationTime", "TimeZoneId" },
                values: new object[,]
                {
                    { 1, false, null, "UTC+1" },
                    { 2, false, null, "UTC+1" },
                    { 3, false, null, "UTC+1" },
                    { 4, false, null, "UTC+1" },
                    { 5, false, null, "UTC+1" },
                    { 6, false, null, "UTC+1" },
                    { 7, false, null, "UTC+1" },
                    { 8, false, null, "UTC+1" },
                    { 9, false, null, "UTC+1" },
                    { 10, false, null, "UTC+1" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParkingSpots");
        }
    }
}
