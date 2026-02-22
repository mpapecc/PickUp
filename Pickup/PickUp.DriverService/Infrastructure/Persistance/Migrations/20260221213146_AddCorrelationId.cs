using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickUp.DriverService.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddCorrelationId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CorrelationId",
                schema: "driverservice",
                table: "Drivers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "driverservice",
                table: "Drivers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                schema: "driverservice",
                table: "Drivers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrelationId",
                schema: "driverservice",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "driverservice",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                schema: "driverservice",
                table: "Drivers");
        }
    }
}
