using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyDine.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminHashUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEClLIJIunylac9YCig9TQqYkf9v/1JAwaW+NUUPO2wormunMgw8mBZu6/9LMTItC0Q==");

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateBooked",
                value: new DateTime(2025, 9, 1, 12, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateBooked",
                value: new DateTime(2025, 9, 1, 18, 0, 0, 0, DateTimeKind.Local));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "admin");

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateBooked",
                value: new DateTime(2025, 8, 30, 12, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateBooked",
                value: new DateTime(2025, 8, 30, 18, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
