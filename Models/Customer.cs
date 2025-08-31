using System.ComponentModel.DataAnnotations;

namespace EasyDine.Models;

public class Customer
{
    [Key]
    public int Id { get; set; }
    
    [Required, MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required, MaxLength(50)]
    public string LastName { get; set; } = string.Empty;
    
    [Required, EmailAddress, MaxLength(100)]
    public string Email { get; set; } = string.Empty;
    
    [Required, Phone, MaxLength(15)]
    // Note: Phone numbers can vary in format, adjust MaxLength as needed
    // Add [RegularExpression] attribute to enforce a specific format
    public string PhoneNumber { get; set; } = string.Empty;
    
    // Navigation property for related bookings
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}