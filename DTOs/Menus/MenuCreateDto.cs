using System.ComponentModel.DataAnnotations;

namespace EasyDine.DTOs.Menus;

public class MenuCreateDto
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Description { get; set; }
}