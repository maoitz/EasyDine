using EasyDine.Extensions;
using EasyDine.Models;
using EasyDine.Repositories;
using EasyDine.Repositories.Bookings;
using Microsoft.Extensions.Options;

namespace EasyDine.Services;

public class BookingService
{
    private readonly IBookingRepository _bookings;
    private readonly IGenericRepository<Table> _tables;
    private readonly IGenericRepository<Customer> _customers;
    private readonly BookingRulesOptions _rules;
    
    public BookingService(
        IBookingRepository bookings,
        IGenericRepository<Table> tables,
        IGenericRepository<Customer> customers,
        IOptions<BookingRulesOptions> rules)
    {
        _bookings = bookings;
        _tables = tables;
        _customers = customers;
        _rules = rules.Value;
    }

    public async Task<(bool IsValid, string Message, Booking normalized)>
        ValidateBookingAsync(Booking booking, int? existingBookingId = null)
    {
        // Normalize input
        booking.DateBooked = booking.DateBooked.TruncateToMinute();

        // Duration checks
        if (booking.DurationMinutes < _rules.MinDurationMinutes ||
            booking.DurationMinutes > _rules.MaxDurationMinutes)
        {
            return (false,
                $"Duration must be between {_rules.MinDurationMinutes} and {_rules.MaxDurationMinutes} minutes.",
                booking);
        }

        // Entities and capacity checks
        var customer = await _customers.GetByIdAsync(booking.CustomerId);
        if (customer is null)
            return (false, "Customer does not exist.", booking);

        var table = await _tables.GetByIdAsync(booking.TableId);
        if (table is null)
            return (false, "Table does not exist.", booking);
        if (booking.TotalGuests > table.Seats)
            return (false, $"Table capacity exceeded. Max capacity is {table.Seats}.", booking);

        // Opening hours check
        var day = booking.DateBooked.Date;
        var openAt = day + _rules.Opening;
        var closedAt = day + _rules.Closing;
        var end = booking.DateBooked.AddMinutes(booking.DurationMinutes);

        if (booking.DateBooked < openAt)
            return (false, $"Bookings must be made after opening time at {openAt:t}.", booking);
        if (end > closedAt)
            return (false, $"Bookings must end before closing time at {closedAt:t}.", booking);

        // Overlap check
        var overlap = await _bookings.HasOverlapAsync(booking.TableId, booking.DateBooked, end, existingBookingId);
        if (overlap)
            return (false, "Booking overlaps with an existing booking for the same table.", booking);

        return (true, "Booking is valid.", booking);
    }
}