using System.ComponentModel.DataAnnotations;

namespace EasyDine.DTOs.MenuItems;

public class MenuItemPutDto
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required, Range(typeof(decimal), "0", "99999.99")]
    public decimal Price { get; set; }
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    [MaxLength(50)]
    public string? Category { get; set; }
    
    [Url, MaxLength(2048)]
    public string? ImageUrl { get; set; }
}