using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyDine.Models;

// This is the model for CustomerTable conjunction table
// It represents a booking made by a customer for a specific table
// It can be extended with additional properties like booking time, status, etc.

public class Booking
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    
    [Required]
    public int TableId { get; set; }
    public Table Table { get; set; } = null!;
    
    [Required]
    public DateTime DateBooked { get; set; }
    
    [Required]
    public int DurationMinutes { get; set; } = 120; // Default duration is 120 minutes (2 hours)
    
    [Required]
    public int TotalGuests { get; set; }
    
    [Required, MaxLength(20)]
    public string Status { get; set; } = "Pending"; // Default status can be "Pending", "Confirmed", "Cancelled"
}