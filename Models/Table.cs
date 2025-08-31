using System.ComponentModel.DataAnnotations;

namespace EasyDine.Models;

public class Table
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int Number { get; set; }
    
    [Required]
    public int Seats { get; set; }
    
    // Navigation property to link Table with Booking
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}