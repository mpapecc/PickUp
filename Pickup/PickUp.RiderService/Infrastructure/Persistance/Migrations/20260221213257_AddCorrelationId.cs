using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickUp.RiderService.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddCorrelationId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "riderservice",
                table: "Riders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                schema: "riderservice",
                table: "Riders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CorrelationId",
                schema: "riderservice",
                table: "RideRequests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "riderservice",
                table: "Riders");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                schema: "riderservice",
                table: "Riders");

            migrationBuilder.DropColumn(
                name: "CorrelationId",
                schema: "riderservice",
                table: "RideRequests");
        }
    }
}
