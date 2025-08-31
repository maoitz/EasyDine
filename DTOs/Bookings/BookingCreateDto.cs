using System.ComponentModel.DataAnnotations;

namespace EasyDine.DTOs.Bookings;

public class BookingCreateDto
{
    [Required]
    public int CustomerId { get; set; }
    
    [Required]
    public int TableId { get; set; }
    
    [Required]
    public DateTime DateBooked { get; set; } // Date and time of the booking
    
    [Required]
    public int? DurationMinutes { get; set; }
    
    [Required]
    public int TotalGuests { get; set; }
}