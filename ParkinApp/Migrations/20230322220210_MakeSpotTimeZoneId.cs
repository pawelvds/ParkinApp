using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkinApp.Migrations
{
    /// <inheritdoc />
    public partial class MakeSpotTimeZoneId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeZoneId",
                table: "ParkingSpots",
                newName: "UserTimeZoneId");

            migrationBuilder.AddColumn<string>(
                name: "SpotTimeZoneId",
                table: "ParkingSpots",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 1,
                column: "SpotTimeZoneId",
                value: "Europe/Warsaw");

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 2,
                column: "SpotTimeZoneId",
                value: "Europe/Warsaw");

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 3,
                column: "SpotTimeZoneId",
                value: "Europe/Warsaw");

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 4,
                column: "SpotTimeZoneId",
                value: "Europe/Warsaw");

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 5,
                column: "SpotTimeZoneId",
                value: "Europe/Warsaw");

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 6,
                column: "SpotTimeZoneId",
                value: "Europe/Warsaw");

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 7,
                column: "SpotTimeZoneId",
                value: "Europe/Warsaw");

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 8,
                column: "SpotTimeZoneId",
                value: "Europe/Warsaw");

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 9,
                column: "SpotTimeZoneId",
                value: "Europe/Warsaw");

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 10,
                column: "SpotTimeZoneId",
                value: "Europe/Warsaw");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpotTimeZoneId",
                table: "ParkingSpots");

            migrationBuilder.RenameColumn(
                name: "UserTimeZoneId",
                table: "ParkingSpots",
                newName: "TimeZoneId");
        }
    }
}
