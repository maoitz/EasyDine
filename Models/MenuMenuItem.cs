namespace EasyDine.Models;

public class MenuMenuItem
{
    public int MenuId { get; set; }
    public Menu Menu { get; set; } = null!;
    
    public int MenuItemId { get; set; }
    public MenuItem MenuItem { get; set; } = null!;
}