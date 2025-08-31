using EasyDine.DTOs;
using EasyDine.DTOs.Tables;
using EasyDine.Models;
using EasyDine.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyDine.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TablesController : ControllerBase
{
    private readonly IGenericRepository<Table> _repository;
    
    public TablesController(IGenericRepository<Table> repository)
    {
        _repository = repository;
    }
    
    // GET: api/Tables
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<TableResponseDto>>>> GetAll()
    {
        var tables = await _repository.GetAllAsync();
        var result = tables.Select(MapToResponse);
        return Ok(ApiResponse<IEnumerable<TableResponseDto>>.Ok(result, "Tables retrieved successfully."));
    }
    
    // GET: api/Tables/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<TableResponseDto>>> GetById(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
            return NotFound(ApiResponse<TableResponseDto>.Fail($"Table with ID {id} not found."));

        return Ok(ApiResponse<TableResponseDto>.Ok(MapToResponse(entity), "Table retrieved successfully."));
    }
    
    // POST: api/Tables
    [HttpPost]
    public async Task<ActionResult<ApiResponse<TableResponseDto>>> Create(TableCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<TableResponseDto>.Fail("Invalid input data."));

        var entity = new Table
        {
            Number = dto.Number,
            Seats = dto.Seats
        };

        await _repository.AddAsync(entity);

        var response = MapToResponse(entity);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id },
            ApiResponse<TableResponseDto>.Ok(response, $"Table with ID {entity.Id} created successfully."));
    }
    
    // PUT: api/Tables/5
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<string>>> Update(int id, TableUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<string>.Fail("Invalid input data."));

        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
            return NotFound(ApiResponse<string>.Fail($"Table with ID {id} not found."));

        entity.Number = dto.Number;
        entity.Seats = dto.Seats;

        await _repository.UpdateAsync(entity);
        
        return Ok(ApiResponse<string>.Ok($"Table with ID {id} updated successfully."));
    }
    
    // DELETE: api/Tables/5
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
    {
        var exists = await _repository.ExistsAsync(id);
        if (!exists)
            return NotFound(ApiResponse<string>.Fail($"Table with ID {id} not found."));

        await _repository.DeleteAsync(id);
        return Ok(ApiResponse<string>.Ok($"Table with ID {id} deleted successfully."));
    }
    
    private static TableResponseDto MapToResponse(Table t) => new()
    {
        Id = t.Id,
        Number = t.Number,
        Seats = t.Seats
    };
}
