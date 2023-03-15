using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ParkinApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    ReservationEndTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TimeZoneId = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingSpots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Login = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    ReservedSpotId = table.Column<int>(type: "INTEGER", nullable: true),
                    UserTimeZoneId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_ParkingSpots_ReservedSpotId",
                        column: x => x.ReservedSpotId,
                        principalTable: "ParkingSpots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "ParkingSpots",
                columns: new[] { "Id", "IsReserved", "ReservationEndTime", "ReservationTime", "TimeZoneId", "UserId" },
                values: new object[,]
                {
                    { 1, false, null, null, "UTC+1", null },
                    { 2, false, null, null, "UTC+1", null },
                    { 3, false, null, null, "UTC+1", null },
                    { 4, false, null, null, "UTC+1", null },
                    { 5, false, null, null, "UTC+1", null },
                    { 6, false, null, null, "UTC+1", null },
                    { 7, false, null, null, "UTC+1", null },
                    { 8, false, null, null, "UTC+1", null },
                    { 9, false, null, null, "UTC+1", null },
                    { 10, false, null, null, "UTC+1", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ReservedSpotId",
                table: "Users",
                column: "ReservedSpotId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ParkingSpots");
        }
    }
}
