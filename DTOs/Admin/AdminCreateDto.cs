using System.ComponentModel.DataAnnotations;

namespace EasyDine.DTOs.Admin;

public class AdminCreateDto
{
    [Required, MinLength(3), MaxLength(50)]
    public string Username { get; set; } = string.Empty;
    
    // Simple for now, can add stronger rules later
    [Required, MinLength(6), MaxLength(100)]
    public string Password { get; set; } = string.Empty;
}