using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkinApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTimeZone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 1,
                column: "TimeZoneId",
                value: "Europe/Warsaw");

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 2,
                column: "TimeZoneId",
                value: "Europe/Warsaw");

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 3,
                column: "TimeZoneId",
                value: "Europe/Warsaw");

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 4,
                column: "TimeZoneId",
                value: "Europe/Warsaw");

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 5,
                column: "TimeZoneId",
                value: "Europe/Warsaw");

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 6,
                column: "TimeZoneId",
                value: "Europe/Warsaw");

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 7,
                column: "TimeZoneId",
                value: "Europe/Warsaw");

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 8,
                column: "TimeZoneId",
                value: "Europe/Warsaw");

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 9,
                column: "TimeZoneId",
                value: "Europe/Warsaw");

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 10,
                column: "TimeZoneId",
                value: "Europe/Warsaw");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 1,
                column: "TimeZoneId",
                value: "UTC+1");

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 2,
                column: "TimeZoneId",
                value: "UTC+1");

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 3,
                column: "TimeZoneId",
                value: "UTC+1");

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 4,
                column: "TimeZoneId",
                value: "UTC+1");

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 5,
                column: "TimeZoneId",
                value: "UTC+1");

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 6,
                column: "TimeZoneId",
                value: "UTC+1");

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 7,
                column: "TimeZoneId",
                value: "UTC+1");

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 8,
                column: "TimeZoneId",
                value: "UTC+1");

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 9,
                column: "TimeZoneId",
                value: "UTC+1");

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 10,
                column: "TimeZoneId",
                value: "UTC+1");
        }
    }
}
