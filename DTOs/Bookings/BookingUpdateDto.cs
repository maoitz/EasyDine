using System.ComponentModel.DataAnnotations;

namespace EasyDine.DTOs.Bookings;

public class BookingUpdateDto
{
    public DateTime? DateBooked { get; set; }
    public int? DurationMinutes { get; set; }
    
    public int? TotalGuests { get; set; }
    
    [MaxLength(20)]
    public string? Status { get; set; } // e.g., "Confirmed", "Cancelled", "Completed"
}