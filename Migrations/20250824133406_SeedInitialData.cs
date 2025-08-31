using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EasyDine.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Menus",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "MenuItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MenuItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "MenuItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Bookings",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "PasswordHash", "Username" },
                values: new object[] { 1, "admin", "admin" });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "john@example.se", "John", "Doe", "1234567890" },
                    { 2, "jane@example.se", "Jane", "Smith", "0987654321" }
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Category", "Description", "ImageUrl", "IsPopular", "Name", "Price" },
                values: new object[,]
                {
                    { 1, null, "A delicious club sandwich with turkey, bacon, lettuce, and tomato.", null, true, "Club Sandwich", 89m },
                    { 2, null, "Classic Italian pasta with creamy sauce, pancetta, and Parmesan cheese.", null, true, "Pasta Carbonara", 129m },
                    { 3, null, "Crisp romaine lettuce with Caesar dressing, croutons, and Parmesan cheese.", null, false, "Caesar Salad", 99m }
                });

            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Our delicious lunch offerings served daily until 13:00.", "Lunch Menu" },
                    { 2, "Our exquisite dinner selections available from 17:00 onwards.", "Dinner Menu" }
                });

            migrationBuilder.InsertData(
                table: "Tables",
                columns: new[] { "Id", "Number", "Seats" },
                values: new object[,]
                {
                    { 1, 1, 2 },
                    { 2, 2, 4 },
                    { 3, 3, 6 }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "CustomerId", "DateBooked", "Status", "TableId", "TimeBooked", "TotalGuests" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 8, 24, 0, 0, 0, 0, DateTimeKind.Local), "Confirmed", 1, new TimeSpan(0, 19, 0, 0, 0), 2 },
                    { 2, 2, new DateTime(2025, 8, 24, 0, 0, 0, 0, DateTimeKind.Local), "Pending", 2, new TimeSpan(0, 20, 0, 0, 0), 4 }
                });

            migrationBuilder.InsertData(
                table: "MenuMenuItems",
                columns: new[] { "MenuId", "MenuItemId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 3 },
                    { 2, 2 },
                    { 2, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MenuMenuItems",
                keyColumns: new[] { "MenuId", "MenuItemId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "MenuMenuItems",
                keyColumns: new[] { "MenuId", "MenuItemId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "MenuMenuItems",
                keyColumns: new[] { "MenuId", "MenuItemId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "MenuMenuItems",
                keyColumns: new[] { "MenuId", "MenuItemId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Menus",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "MenuItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MenuItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "MenuItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);
        }
    }
}
