using EasyDine.Data;
using EasyDine.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyDine.Repositories.Bookings;

public class BookingRepository : GenericRepository<Booking>, IBookingRepository
{
    private readonly AppDbContext _context;

    public BookingRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Booking>> GetAllWithIncludesAsync()
    {
        return await _context.Bookings
            .Include(b => b.Customer)
            .Include(b => b.Table)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Booking?> GetByIdWithIncludesAsync(int id)
    {
        return await _context.Bookings
            .Include(b => b.Customer)
            .Include(b => b.Table)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<bool> HasOverlapAsync(int tableId, DateTime newStart, DateTime newEnd,
        int? excludeBookingId = null)
    {
        return await _context.Bookings
            .AsNoTracking()
            .Where(b => b.TableId == tableId && (excludeBookingId == null || b.Id != excludeBookingId.Value))
            .AnyAsync(b => newStart < b.DateBooked.AddMinutes(b.DurationMinutes) && newEnd > b.DateBooked);

    }
}