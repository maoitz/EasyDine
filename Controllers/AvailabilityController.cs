using EasyDine.DTOs;
using EasyDine.DTOs.Availability;
using EasyDine.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EasyDine.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AvailabilityController : ControllerBase
{
    private readonly AvailabilityService _service;
    private readonly BookingRulesOptions _rules;

    public AvailabilityController(AvailabilityService service, IOptions<BookingRulesOptions> rules)
    {
        _service = service;
        _rules = rules.Value;
    }

    // GET: api/availability/available?start=2025-09-01T18:00:00&durationMinutes=120&guests=4
    [HttpGet("available")]
    public async Task<ActionResult<ApiResponse<IEnumerable<TableAvailabilityDto>>>> GetAvailable(
        [FromQuery] DateTime start,
        [FromQuery] int durationMinutes,
        [FromQuery] int guests)
    {
        if (guests <= 0) return BadRequest(ApiResponse<IEnumerable<TableAvailabilityDto>>.Fail("Guests must be > 0."));
        if (durationMinutes < _rules.MinDurationMinutes || durationMinutes > _rules.MaxDurationMinutes)
            return BadRequest(ApiResponse<IEnumerable<TableAvailabilityDto>>.Fail(
                $"Duration must be {_rules.MinDurationMinutes}-{_rules.MaxDurationMinutes} minutes."));

        // Opening-hours check
        var day = start.Date;
        var openAt  = day + _rules.Opening;
        var closeAt = day + _rules.Closing;
        var end = new DateTime(start.Year, start.Month, start.Day, start.Hour, start.Minute, 0, start.Kind)
                  .AddMinutes(durationMinutes);
        if (start < openAt || end > closeAt)
            return BadRequest(ApiResponse<IEnumerable<TableAvailabilityDto>>.Fail(
                $"Booking must be within opening hours {openAt:t}–{closeAt:t}."));

        var list = await _service.GetAvailableTablesAsync(start, durationMinutes, guests);
        return Ok(ApiResponse<IEnumerable<TableAvailabilityDto>>.Ok(list, "Available tables retrieved."));
    }

    // GET: api/availability/details?start=2025-09-01T18:00:00&durationMinutes=120&guests=4
    [HttpGet("details")]
    public async Task<ActionResult<ApiResponse<IEnumerable<TableAvailabilityDetailDto>>>> GetDetails(
        [FromQuery] DateTime start,
        [FromQuery] int durationMinutes,
        [FromQuery] int guests)
    {
        if (guests <= 0) return BadRequest(ApiResponse<IEnumerable<TableAvailabilityDetailDto>>.Fail("Guests must be > 0."));
        var list = await _service.GetAvailabilityDetailsAsync(start, durationMinutes, guests);
        return Ok(ApiResponse<IEnumerable<TableAvailabilityDetailDto>>.Ok(list, "Availability details retrieved."));
    }
}
