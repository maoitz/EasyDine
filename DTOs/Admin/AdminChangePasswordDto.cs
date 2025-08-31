using System.ComponentModel.DataAnnotations;

namespace EasyDine.DTOs.Admin;

public class AdminChangePasswordDto
{
    [Required, MinLength(6), MaxLength(100)]
    public string NewPassword { get; set; } = string.Empty;
}