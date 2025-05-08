using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eyemergency.Migrations
{
    /// <inheritdoc />
    public partial class AddEmergencyFieldsToVehicle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrafficEvents");

            migrationBuilder.AlterColumn<string>(
                name: "TagId",
                table: "Vehicle",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ApproachLane",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Vehicle",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmergencyVehicle",
                table: "Vehicle",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "EmergencyStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ActivationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TriggeredLane = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencyStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmergencyStatus_Vehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrafficLightLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChangeTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PreviousState = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NewState = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TriggeredByVehicleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrafficLightLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrafficLightLogs_Vehicle_TriggeredByVehicleId",
                        column: x => x.TriggeredByVehicleId,
                        principalTable: "Vehicle",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyStatus_VehicleId",
                table: "EmergencyStatus",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_TrafficLightLogs_TriggeredByVehicleId",
                table: "TrafficLightLogs",
                column: "TriggeredByVehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmergencyStatus");

            migrationBuilder.DropTable(
                name: "TrafficLightLogs");

            migrationBuilder.DropColumn(
                name: "ApproachLane",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "IsEmergencyVehicle",
                table: "Vehicle");

            migrationBuilder.AlterColumn<string>(
                name: "TagId",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateTable(
                name: "TrafficEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    AccessGranted = table.Column<bool>(type: "bit", nullable: false),
                    TagId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VehicleType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrafficEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrafficEvents_Vehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrafficEvents_VehicleId",
                table: "TrafficEvents",
                column: "VehicleId");
        }
    }
}
