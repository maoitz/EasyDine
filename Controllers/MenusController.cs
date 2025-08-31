using EasyDine.DTOs;
using EasyDine.DTOs.Menus;
using EasyDine.Models;
using EasyDine.Repositories;
using EasyDine.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyDine.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenusController : ControllerBase
{
    private readonly IGenericRepository<Menu> _repository;
    private readonly MenuService _menuService;
    
    public MenusController(IGenericRepository<Menu> repository, MenuService menuService)
    {
        _repository = repository;
        _menuService = menuService;
    }
    
    // GET: api/menus
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<MenuResponseDto>>>> GetAll()
    {
        var menus = await _repository.GetAllAsync();
        var result = menus.Select(ToDto);
        return Ok(ApiResponse<IEnumerable<MenuResponseDto>>.Ok(result, "All Menus retrieved successfully."));
    }

    // GET: api/menus/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<MenuResponseDto>>> GetById(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
            return NotFound(ApiResponse<MenuResponseDto>.Fail($"Menu with ID {id} not found."));

        return Ok(ApiResponse<MenuResponseDto>.Ok(ToDto(entity), $"Menu with ID {id} retrieved successfully."));
    }
    
    // GET: api/menus/5/items?category=Main&isPopular=true
    [HttpGet("{id:int}/items")]
    public async Task<ActionResult<ApiResponse<MenuWithItemsResponseDto>>> GetWithItems(
        int id, [FromQuery] string? category, [FromQuery] bool? isPopular)
    {
        category = string.IsNullOrWhiteSpace(category) ? null : category.Trim();
        
        var dto = await _menuService.GetMenuWithItemsAsync(id, category, isPopular);
        if (dto is null)
            return NotFound(ApiResponse<MenuWithItemsResponseDto>.Fail($"Menu with ID {id} not found."));
        
        return Ok(ApiResponse<MenuWithItemsResponseDto>.Ok(dto, $"Menu {id} with items retrieved successfully."));
    }

    // POST: api/menus
    [HttpPost]
    public async Task<ActionResult<ApiResponse<MenuResponseDto>>> Create(MenuCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<MenuResponseDto>.Fail("Invalid input data."));

        var entity = new Menu
        {
            Name = dto.Name,
            Description = dto.Description
        };

        await _repository.AddAsync(entity);
        var response = ToDto(entity);

        return CreatedAtAction(nameof(GetById), new { id = entity.Id },
            ApiResponse<MenuResponseDto>.Ok(response, $"Menu with ID {entity.Id} created successfully."));
    }

    // PUT: api/menus/5
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<string>>> Update(int id, MenuUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<string>.Fail("Invalid input data."));

        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
            return NotFound(ApiResponse<string>.Fail($"Menu with ID {id} not found."));

        entity.Name = dto.Name;
        entity.Description = dto.Description;

        await _repository.UpdateAsync(entity);
        return Ok(ApiResponse<string>.Ok($"Menu with ID {id} updated successfully."));
    }

    // DELETE: api/menus/5
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
    {
        var exists = await _repository.ExistsAsync(id);
        if (!exists)
            return NotFound(ApiResponse<string>.Fail($"Menu with ID {id} not found."));

        await _repository.DeleteAsync(id);
        return Ok(ApiResponse<string>.Ok($"Menu with ID {id} deleted successfully."));
    }
    
    // Helper method to convert Menu to MenuResponseDto
    private static MenuResponseDto ToDto(Menu m) => new()
    {
        Id = m.Id,
        Name = m.Name,
        Description = m.Description
    };
}