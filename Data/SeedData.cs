using EasyDine.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyDine.Data;

public static class SeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        // -- Tables --
        modelBuilder.Entity<Table>().HasData(
            new Table { Id = 1, Number = 1, Seats = 2 },
            new Table { Id = 2, Number = 2, Seats = 4 },
            new Table { Id = 3, Number = 3, Seats = 6 }
        );

        // -- Admins --
        modelBuilder.Entity<Admin>().HasData(
            new Admin
            {
                Id = 1,
                Username = "admin",
                PasswordHash = "AQAAAAIAAYagAAAAEClLIJIunylac9YCig9TQqYkf9v/1JAwaW+NUUPO2wormunMgw8mBZu6/9LMTItC0Q=="
                // Password = "Admin123!"
            }
        );

        // -- Customers --  
        modelBuilder.Entity<Customer>().HasData(
            new Customer
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.se",
                PhoneNumber = "1234567890"
            },
            new Customer
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.se",
                PhoneNumber = "0987654321"
            }
        );

        // -- Bookings (fixed demo dates for consistency) --
        modelBuilder.Entity<Booking>().HasData(
            new Booking
            {
                Id = 1,
                CustomerId = 1,
                TableId = 1,
                DateBooked = new DateTime(2025, 09, 01, 12, 00, 00), // Sept 1, 2025 at 12:00
                DurationMinutes = 120,
                TotalGuests = 2,
                Status = "Confirmed"
            },
            new Booking
            {
                Id = 2,
                CustomerId = 2,
                TableId = 2,
                DateBooked = new DateTime(2025, 09, 01, 18, 00, 00), // Sept 1, 2025 at 18:00
                DurationMinutes = 120,
                TotalGuests = 4,
                Status = "Pending"
            }
        );

        // -- Menus --
        modelBuilder.Entity<Menu>().HasData(
            new Menu
            {
                Id = 1,
                Name = "Lunch Menu",
                Description = "Our delicious lunch offerings served daily until 13:00."
            },
            new Menu
            {
                Id = 2,
                Name = "Dinner Menu",
                Description = "Our exquisite dinner selections available from 17:00 onwards."
            }
        );

        // -- Menu Items --
        modelBuilder.Entity<MenuItem>().HasData(
            new MenuItem
            {
                Id = 1,
                Name = "Club Sandwich",
                Price = 89,
                Description = "A delicious club sandwich with turkey, bacon, lettuce, and tomato."
            },
            new MenuItem
            {
                Id = 2,
                Name = "Pasta Carbonara",
                Price = 129,
                Description = "Classic Italian pasta with creamy sauce, pancetta, and Parmesan cheese."
            },
            new MenuItem
            {
                Id = 3,
                Name = "Caesar Salad",
                Price = 99,
                Description = "Crisp romaine lettuce with Caesar dressing, croutons, and Parmesan cheese."
            }
        );

        // -- Menu/MenuItems Relations --
        modelBuilder.Entity<MenuMenuItem>().HasData(
            new { MenuId = 1, MenuItemId = 1 }, // Club Sandwich in Lunch Menu
            new { MenuId = 1, MenuItemId = 3 }, // Caesar Salad in Lunch Menu
            new { MenuId = 2, MenuItemId = 2 }, // Pasta Carbonara in Dinner Menu
            new { MenuId = 2, MenuItemId = 3 }  // Caesar Salad in Dinner Menu
        );
    }
}
