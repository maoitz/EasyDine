namespace EasyDine.DTOs.MenuItems;

public class MenuItemResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsPopular { get; set; } // Indicates if the item is marked as popular
}