using System.ComponentModel.DataAnnotations;

namespace EasyDine.Models;

public class Menu
{
    [Key] 
    public int Id { get; set; }

    [Required, MaxLength(100)] 
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Description { get; set; }

    // Navigation property to link Menu with MenuItems
    public ICollection<MenuMenuItem> MenuMenuItems { get; set; } = new List<MenuMenuItem>();
}