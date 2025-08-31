using System.ComponentModel.DataAnnotations;

namespace EasyDine.DTOs.Customers;

public class CustomerUpdateDto
{
    [Required, MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required, MaxLength(50)]
    public string LastName { get; set; } = string.Empty;
    
    [Required, MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required, MaxLength(15)]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;
}