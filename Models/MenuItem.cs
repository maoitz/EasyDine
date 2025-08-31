using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace EasyDine.Models;

public class MenuItem
{
    [Key]
    public int Id { get; set; }
    
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Precision(10, 2)]
    public decimal Price { get; set; }
    
    [MaxLength(500)]
    public string? Description { get; set; } // Optional
    
    [MaxLength(50)]
    public string? Category { get; set; } // Optional
    
    [MaxLength(2048)]
    public string? ImageUrl { get; set; } // Optional, URL to the image of the menu item
    
    // -- Popularity inputs --
    public int PurchaseCount { get; set; } = 0; // Number of times this item has been purchased
    
    public bool? AdminIsPopularOverride { get; set; } // If null => use derived rule; if true/false => override with admin decision
    
    public int PopularityThreshold { get; set; } = 50; // Default threshold for popularity
    
    [NotMapped]
    public bool IsPopular => AdminIsPopularOverride ?? (PurchaseCount >= PopularityThreshold); // Derived property
    
    [JsonIgnore]
    public ICollection<MenuMenuItem> MenuMenuItems { get; set; } = new List<MenuMenuItem>(); // Navigation property
}