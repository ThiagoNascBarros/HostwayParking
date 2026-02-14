using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HostwayParking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixVehicleFKNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vehicle_parking_ParkingId",
                table: "vehicle");

            migrationBuilder.AlterColumn<int>(
                name: "ParkingId",
                table: "vehicle",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_vehicle_parking_ParkingId",
                table: "vehicle",
                column: "ParkingId",
                principalTable: "parking",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vehicle_parking_ParkingId",
                table: "vehicle");

            migrationBuilder.AlterColumn<int>(
                name: "ParkingId",
                table: "vehicle",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_vehicle_parking_ParkingId",
                table: "vehicle",
                column: "ParkingId",
                principalTable: "parking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
