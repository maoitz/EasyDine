using EasyDine.Data;
using EasyDine.DTOs.Menus;
using Microsoft.EntityFrameworkCore;

namespace EasyDine.Services;

public class MenuService
{
    private readonly AppDbContext _context;
    public MenuService(AppDbContext context) => _context = context;
    
    public async Task<MenuWithItemsResponseDto?> GetMenuWithItemsAsync(
        int menuId, string? category = null, bool? IsPopular = null)
    {
        var menu = await _context.Menus.AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == menuId);
        if (menu == null) return null;
        
        // Pull menu items via join table, then apply popularity in-memory
        var items = await _context.MenuMenuItems
            .AsNoTracking()
            .Where(x => x.MenuId == menuId &&
                        (category == null || x.MenuItem.Category == category))
            .Select(x => x.MenuItem)
            .ToListAsync();
        
        if (IsPopular.HasValue)
            items = items.Where(i => i.IsPopular == IsPopular.Value).ToList();

        return new MenuWithItemsResponseDto
        {
            Id = menu.Id,
            Name = menu.Name,
            Description = menu.Description,
            Items = items.Select(i => new MenuItemSummaryDto
            {
                Id = i.Id,
                Name = i.Name,
                Price = i.Price,
                Description = i.Description,
                IsPopular = i.IsPopular,
                ImageUrl = i.ImageUrl,
                Category = i.Category
            }).ToList()
        };
    }
}