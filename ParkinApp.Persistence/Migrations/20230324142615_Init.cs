using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ParkinApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Login = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "BLOB", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "BLOB", nullable: false),
                    ReservedSpotId = table.Column<int>(type: "INTEGER", nullable: true),
                    UserTimeZoneId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParkingSpots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsReserved = table.Column<bool>(type: "INTEGER", nullable: false),
                    ReservationTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ReservationEndTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    SpotTimeZoneId = table.Column<string>(type: "TEXT", nullable: false),
                    UserTimeZoneId = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingSpots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParkingSpots_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "ParkingSpots",
                columns: new[] { "Id", "IsReserved", "ReservationEndTime", "ReservationTime", "SpotTimeZoneId", "UserId", "UserTimeZoneId" },
                values: new object[,]
                {
                    { 1, false, null, null, "Europe/Warsaw", null, "Europe/Warsaw" },
                    { 2, false, null, null, "Europe/Warsaw", null, "Europe/Warsaw" },
                    { 3, false, null, null, "Europe/Warsaw", null, "Europe/Warsaw" },
                    { 4, false, null, null, "Europe/Warsaw", null, "Europe/Warsaw" },
                    { 5, false, null, null, "Europe/Warsaw", null, "Europe/Warsaw" },
                    { 6, false, null, null, "Europe/Warsaw", null, "Europe/Warsaw" },
                    { 7, false, null, null, "Europe/Warsaw", null, "Europe/Warsaw" },
                    { 8, false, null, null, "Europe/Warsaw", null, "Europe/Warsaw" },
                    { 9, false, null, null, "Europe/Warsaw", null, "Europe/Warsaw" },
                    { 10, false, null, null, "Europe/Warsaw", null, "Europe/Warsaw" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSpots_UserId",
                table: "ParkingSpots",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParkingSpots");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
