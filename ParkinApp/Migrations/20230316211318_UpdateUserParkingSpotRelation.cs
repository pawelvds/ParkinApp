using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkinApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserParkingSpotRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_ParkingSpots_ReservedSpotId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ReservedSpotId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSpots_UserId",
                table: "ParkingSpots",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSpots_Users_UserId",
                table: "ParkingSpots",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSpots_Users_UserId",
                table: "ParkingSpots");

            migrationBuilder.DropIndex(
                name: "IX_Users_Login",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_ParkingSpots_UserId",
                table: "ParkingSpots");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ReservedSpotId",
                table: "Users",
                column: "ReservedSpotId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ParkingSpots_ReservedSpotId",
                table: "Users",
                column: "ReservedSpotId",
                principalTable: "ParkingSpots",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
