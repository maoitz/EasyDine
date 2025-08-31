using EasyDine.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyDine.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<MenuMenuItem> MenuMenuItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Many-to-many Menu <-> MenuItem
            modelBuilder.Entity<MenuMenuItem>()
                .HasKey(mmi => new { mmi.MenuId, mmi.MenuItemId });

            modelBuilder.Entity<MenuMenuItem>()
                .HasOne(mmi => mmi.Menu)
                .WithMany(m => m.MenuMenuItems)
                .HasForeignKey(mmi => mmi.MenuId);

            modelBuilder.Entity<MenuMenuItem>()
                .HasOne(mmi => mmi.MenuItem)
                .WithMany(mi => mi.MenuMenuItems)
                .HasForeignKey(mmi => mmi.MenuItemId);

            // Unique constraints
            modelBuilder.Entity<Table>()
                .HasIndex(t => t.Number)
                .IsUnique();

            modelBuilder.Entity<Admin>()
                .HasIndex(a => a.Username)
                .IsUnique();
            
            modelBuilder.Entity<Booking>()
                .HasIndex(b => new { b.TableId, b.DateBooked });


            // Data Seeding
            SeedData.Seed(modelBuilder);
        }
    }
}