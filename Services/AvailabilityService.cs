using EasyDine.Data;
using EasyDine.DTOs.Availability;
using EasyDine.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EasyDine.Services;


public class AvailabilityService
{
    private readonly AppDbContext _context;
    private readonly BookingRulesOptions _rules;
    
    public AvailabilityService(AppDbContext context, IOptions<BookingRulesOptions> rules)
    {
        _context = context;
        _rules = rules.Value;
    }

    public async Task<List<TableAvailabilityDto>> GetAvailableTablesAsync(DateTime start, int durationMinutes, int guests)
    {
        start = new DateTime(start.Year, start.Month, start.Day, start.Hour, start.Minute, 0, start.Kind);
        var end = start.AddMinutes(durationMinutes);

        // Basic checks
        var candidates = await _context.Tables.AsNoTracking()
            .Where(t => t.Seats >= guests)
            .Select(t => new { t.Id, t.Number, t.Seats })
            .ToListAsync();

        // Same-day bookings only
        var day = start.Date;
        var tableIds = candidates.Select(c => c.Id).ToList();

        var sameDay = await _context.Bookings.AsNoTracking()
            .Where(b => tableIds.Contains(b.TableId) && b.DateBooked.Date == day)
            .Select(b => new { b.TableId, b.DateBooked, b.DurationMinutes })
            .ToListAsync();

        // Filter by no overlap
        var free = new List<TableAvailabilityDto>();
        foreach (var t in candidates)
        {
            var overlaps = sameDay.Any(b =>
                b.TableId == t.Id &&
                start < b.DateBooked.AddMinutes(b.DurationMinutes) &&
                end   > b.DateBooked);

            if (!overlaps)
            {
                free.Add(new TableAvailabilityDto() { Id = t.Id, Number = t.Number, Seats = t.Seats });
            }
        }

        return free;
    }
    
    public async Task<List<TableAvailabilityDetailDto>> GetAvailabilityDetailsAsync(DateTime start, int durationMinutes, int guests)
    {
        start = new DateTime(start.Year, start.Month, start.Day, start.Hour, start.Minute, 0, start.Kind);
        var end = start.AddMinutes(durationMinutes);
        var day = start.Date;

        var candidates = await _context.Tables.AsNoTracking()
            .Where(t => t.Seats >= guests)
            .ToListAsync();

        var sameDay = await _context.Bookings.AsNoTracking()
            .Where(b => b.DateBooked.Date == day)
            .ToListAsync();

        var result = new List<TableAvailabilityDetailDto>();

        foreach (var table in candidates)
        {
            // bookings for this table
            var bookings = sameDay.Where(b => b.TableId == table.Id).ToList();

            // find overlaps with requested window
            var overlapping = bookings
                .Where(b => start < b.DateBooked.AddMinutes(b.DurationMinutes) &&
                            end   > b.DateBooked)
                .ToList();

            DateTime? nextAvailable = null;
            if (overlapping.Count > 0)
            {
                // when is it free again? after the last overlapping booking ends
                nextAvailable = overlapping
                    .Select(b => b.DateBooked.AddMinutes(b.DurationMinutes))
                    .Max();
            }

            result.Add(new TableAvailabilityDetailDto
            {
                Id = table.Id,
                Number = table.Number,
                Seats = table.Seats,
                IsAvailable = overlapping.Count == 0,
                NextAvailableAt = nextAvailable
            });
        }

        return result;
    }
}