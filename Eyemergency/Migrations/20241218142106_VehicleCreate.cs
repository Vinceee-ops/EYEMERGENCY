using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eyemergency.Migrations
{
    /// <inheritdoc />
    public partial class VehicleCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRegistered",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "Vehicle");

            migrationBuilder.RenameColumn(
                name: "VehicleCode",
                table: "Vehicle",
                newName: "VehicleName");

            migrationBuilder.RenameColumn(
                name: "PlateNumber",
                table: "Vehicle",
                newName: "TagId");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegisteredAt",
                table: "Vehicle",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegisteredAt",
                table: "Vehicle");

            migrationBuilder.RenameColumn(
                name: "VehicleName",
                table: "Vehicle",
                newName: "VehicleCode");

            migrationBuilder.RenameColumn(
                name: "TagId",
                table: "Vehicle",
                newName: "PlateNumber");

            migrationBuilder.AddColumn<bool>(
                name: "IsRegistered",
                table: "Vehicle",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
