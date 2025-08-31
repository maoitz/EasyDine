using EasyDine.Models;

namespace EasyDine.Repositories.Bookings;

public interface IBookingRepository : IGenericRepository<Booking>
{
    Task<IEnumerable<Booking>> GetAllWithIncludesAsync();
    Task<Booking?> GetByIdWithIncludesAsync(int id);
   Task<bool> HasOverlapAsync(int tableId, DateTime start, DateTime end, int? excludeBookingId = null);
}