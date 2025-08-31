using System.ComponentModel.DataAnnotations;

namespace EasyDine.DTOs.Customers;

public class CustomerCreateDto
{
    [Required, MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required, Phone, MaxLength(15)]
    public string PhoneNumber { get; set; } = string.Empty;
}