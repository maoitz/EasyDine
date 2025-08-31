namespace EasyDine.DTOs.Menus;

public class MenuWithItemsResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<MenuItemSummaryDto> Items { get; set; } = new();
}

public class MenuItemSummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public bool IsPopular { get; set; }
    public string? ImageUrl { get; set; }
}