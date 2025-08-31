using EasyDine.DTOs;
using EasyDine.DTOs.Bookings;
using EasyDine.Models;
using EasyDine.Repositories.Bookings;
using EasyDine.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyDine.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingRepository _repository;
    private readonly BookingService _service;

    public BookingsController(IBookingRepository repository, BookingService service)
    {
        _repository = repository;
        _service = service;
    }

    // GET: api/bookings
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<BookingResponseDto>>>> GetAll()
    {
        var bookings = await _repository.GetAllWithIncludesAsync();
        var result = bookings.Select(MapToResponse);
        return Ok(ApiResponse<IEnumerable<BookingResponseDto>>.Ok(result, "All Bookings retrieved successfully."));
    }

    // GET: api/bookings/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<BookingResponseDto>>> GetById(int id)
    {
        var booking = await _repository.GetByIdWithIncludesAsync(id);
        if (booking is null)
            return NotFound(ApiResponse<BookingResponseDto>.Fail($"Booking with ID {id} not found."));

        return Ok(ApiResponse<BookingResponseDto>.Ok(MapToResponse(booking), $"Booking with ID {id} retrieved successfully."));
    }

    // POST: api/bookings
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<BookingResponseDto>>> Create(BookingCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<BookingResponseDto>.Fail("Invalid input data."));

        var booking = new Booking
        {
            CustomerId = dto.CustomerId,
            TableId = dto.TableId,
            DateBooked = dto.DateBooked,
            DurationMinutes = dto.DurationMinutes ?? 120, // Default to 120 minutes if not provided
            TotalGuests = dto.TotalGuests,
            Status = "Pending"
        };

        var validation = await _service.ValidateBookingAsync(booking);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<BookingResponseDto>.Fail(validation.Message));

        await _repository.AddAsync(booking);

        var createdBooking = await _repository.GetByIdWithIncludesAsync(booking.Id) ?? booking;
        var response = MapToResponse(createdBooking);
        
        return CreatedAtAction(nameof(GetById), new { id = booking.Id },
            ApiResponse<BookingResponseDto>.Ok(response, "Booking created successfully."));
    }
    
    // PUT: api/bookings/5
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<string>>> Update(int id, BookingUpdateDto dto)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
            return NotFound(ApiResponse<string>.Fail($"Booking with ID {id} not found."));
        
        // Apply updates only for provided fields
        if (dto.DateBooked.HasValue)
            entity.DateBooked = dto.DateBooked.Value;
        if (dto.TotalGuests.HasValue)
            entity.TotalGuests = dto.TotalGuests.Value;
        if (!string.IsNullOrWhiteSpace(dto.Status))
            entity.Status = dto.Status;
        
        // Re-validate the updated booking if date, time, or guests changed
        if (dto.DateBooked.HasValue || dto.TotalGuests.HasValue)
        {
            var validation = await _service.ValidateBookingAsync(entity, existingBookingId: entity.Id);
            if (!validation.IsValid)
                return BadRequest(ApiResponse<string>.Fail(validation.Message));
        }
        
        await _repository.UpdateAsync(entity);
        return Ok(ApiResponse<string>.Ok("Booking updated successfully."));
    }

    private static BookingResponseDto MapToResponse(Booking b) => new()
    {
        Id = b.Id,
        CustomerName = $"{b.Customer.FirstName} {b.Customer.LastName}",
        TableNumber = b.Table.Number,
        StartTime = b.DateBooked,
        EndTime = b.DateBooked.AddMinutes(b.DurationMinutes),
        TotalGuests = b.TotalGuests,
        Status = b.Status
    };
}