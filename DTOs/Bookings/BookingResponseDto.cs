namespace EasyDine.DTOs.Bookings;

public class BookingResponseDto
{
    public int Id { get; set; }
    public string CustomerName { get; set; } // Full name of the customer
    public int TableNumber { get; set; } // Table number
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int TotalGuests { get; set; }
    public string Status { get; set; } // e.g., "Confirmed", "Cancelled", "Completed"
}