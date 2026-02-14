using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HostwayParking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddParkingSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SessionParkings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VehicleId = table.Column<int>(type: "INTEGER", nullable: false),
                    EntryTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExitTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AmountCharged = table.Column<decimal>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionParkings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionParkings_vehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "vehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_Plate",
                table: "vehicle",
                column: "Plate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SessionParkings_VehicleId",
                table: "SessionParkings",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessionParkings");

            migrationBuilder.DropIndex(
                name: "IX_vehicle_Plate",
                table: "vehicle");
        }
    }
}
