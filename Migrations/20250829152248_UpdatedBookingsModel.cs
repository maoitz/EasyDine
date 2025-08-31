using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyDine.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedBookingsModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeBooked",
                table: "Bookings");

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateBooked", "DurationMinutes" },
                values: new object[] { new DateTime(2025, 8, 30, 12, 0, 0, 0, DateTimeKind.Local), 120 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateBooked", "DurationMinutes" },
                values: new object[] { new DateTime(2025, 8, 30, 18, 0, 0, 0, DateTimeKind.Local), 120 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "Bookings");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeBooked",
                table: "Bookings",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateBooked", "TimeBooked" },
                values: new object[] { new DateTime(2025, 8, 28, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 19, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateBooked", "TimeBooked" },
                values: new object[] { new DateTime(2025, 8, 28, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 20, 0, 0, 0) });
        }
    }
}
