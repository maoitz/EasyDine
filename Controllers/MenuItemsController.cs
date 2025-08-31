using EasyDine.DTOs;
using EasyDine.DTOs.MenuItems;
using EasyDine.Models;
using EasyDine.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyDine.Controllers;

// CRUD operations for MenuItem entity with filtering
// TO DO: Merge PUT and PATCH into a single endpoint

[ApiController]
[Route("api/[controller]")]
public class MenuItemsController : ControllerBase
{
    private readonly IGenericRepository<MenuItem> _repository;
    
    public MenuItemsController(IGenericRepository<MenuItem> repository) => _repository = repository;
    
    // GET: api/menuitems?category=Main&isPopular=true&minPrice=50&maxPrice=200
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<MenuItemResponseDto>>>> GetAll(
        [FromQuery] string? category,
        [FromQuery] bool? isPopular,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice)
    {
        // Filter by category and price range
        var items = await _repository.FindAsync(mi =>
            (category == null || mi.Category == category) &&
            (minPrice == null || mi.Price >= minPrice) &&
            (maxPrice == null || mi.Price <= maxPrice)
        );

        // Popularity is derived (NotMapped), so filter in-memory
        if (isPopular.HasValue)
            items = items.Where
                (mi => (mi.AdminIsPopularOverride ?? (mi.PurchaseCount >= mi.PopularityThreshold)) == isPopular.Value);
        var result = items.Select(ToDto);
        return Ok(ApiResponse<IEnumerable<MenuItemResponseDto>>.Ok(result, "Menu items retrieved successfully"));
    }
    
    // GET: api/menuitems/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<MenuItemResponseDto>>> GetById(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
            return NotFound(ApiResponse<MenuItemResponseDto>.Fail($"Menu item with ID {id} not found"));

        return Ok(ApiResponse<MenuItemResponseDto>.Ok(ToDto(entity), "Menu item retrieved successfully"));
    }
    
    // POST: api/menuitems
    [HttpPost]
    public async Task<ActionResult<ApiResponse<MenuItemResponseDto>>> Create(MenuItemCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<MenuItemResponseDto>.Fail("Invalid input data"));
        var entity = new MenuItem
        {
            Name = dto.Name,
            Price = dto.Price,
            Description = dto.Description,
            Category = dto.Category,
            ImageUrl = dto.ImageUrl
        };

        await _repository.AddAsync(entity);

        return CreatedAtAction(nameof(GetById), new { id = entity.Id },
            ApiResponse<MenuItemResponseDto>.Ok(ToDto(entity), "Menu item created successfully"));
    }
    
    // PUT: Full Replacement
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<string>>> Put(int id, MenuItemPutDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<string>.Fail("Invalid input data"));
        
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
            return NotFound(ApiResponse<string>.Fail($"Menu item with ID {id} not found"));
        
        // Full replacement of fields
        entity.Name = dto.Name;
        entity.Price = dto.Price;
        entity.Description = dto.Description;
        entity.Category = dto.Category;
        entity.ImageUrl = dto.ImageUrl;
        
        await _repository.UpdateAsync(entity);
        return Ok(ApiResponse<string>.Ok($"Menu item with ID {id} updated successfully"));
    }
    
    // PATCH: Partial Update
    [HttpPatch("{id:int}")]
    public async Task<ActionResult<ApiResponse<string>>> Patch(int id, MenuItemUpdateDto dto)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
            return NotFound(ApiResponse<string>.Fail($"Menu item with ID {id} not found"));

        var changed = false;

        // Switch case?
        if (dto.Name is not null) { entity.Name = dto.Name; changed = true; }
        if (dto.Price.HasValue) { entity.Price = dto.Price.Value; changed = true; }
        if (dto.Description is not null) { entity.Description = dto.Description; changed = true; }
        if (dto.Category is not null) { entity.Category = dto.Category; changed = true; }
        if (dto.ImageUrl is not null) { entity.ImageUrl = dto.ImageUrl; changed = true; }
        
        if (!changed)
            return BadRequest(ApiResponse<string>.Fail("No valid fields provided for update"));

        await _repository.UpdateAsync(entity);
        return Ok(ApiResponse<string>.Ok($"Menu item with ID {id} updated successfully"));
    }
    
    // DELETE: api/menuitems/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
    {
        var exists = await _repository.ExistsAsync(id);
        if (!exists)
            return NotFound(ApiResponse<string>.Fail($"Menu item with ID {id} not found"));

        await _repository.DeleteAsync(id);
        return Ok(ApiResponse<string>.Ok("Menu item deleted successfully"));
    }
    
    // Helper
    private static MenuItemResponseDto ToDto(MenuItem mi) => new()
    {
        Id = mi.Id,
        Name = mi.Name,
        Price = mi.Price,
        Description = mi.Description,
        Category = mi.Category,
        ImageUrl = mi.ImageUrl,
        IsPopular = mi.IsPopular // Derived property
    };

}