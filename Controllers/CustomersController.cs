using EasyDine.DTOs;
using EasyDine.DTOs.Customers;
using EasyDine.Models;
using EasyDine.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyDine.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IGenericRepository<Customer> _repository;
    
    public CustomersController(IGenericRepository<Customer> repository)
    {
        _repository = repository;
    }
    
    // GET: api/Customers
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CustomerResponseDto>>>> GetAll()
    {
        var customers = await _repository.GetAllAsync();
        var result = customers.Select(MapToResponse);
        return Ok(ApiResponse<IEnumerable<CustomerResponseDto>>.Ok(result, "Customers retrieved successfully."));
    }
    
    // GET: api/Customers/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<CustomerResponseDto>>> GetById(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
            return NotFound(ApiResponse<CustomerResponseDto>.Fail($"Customer with ID {id} not found."));
        
        return Ok(ApiResponse<CustomerResponseDto>.Ok(MapToResponse(entity), $"Customer with ID {id} retrieved successfully."));
    }
    
    // POST: api/Customers
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CustomerResponseDto>>> Create(CustomerCreateDto dto)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ApiResponse<CustomerResponseDto>.Fail("Invalid input data."));

        var entity = new Customer
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber
        };

        await _repository.AddAsync(entity);

        var response = MapToResponse(entity);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, 
            ApiResponse<CustomerResponseDto>.Ok(response, $"Customer with ID {entity.Id} created successfully."));
    }
    
    // PUT: api/Customers/5
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<string>>> Update(int id, CustomerUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<string>.Fail("Invalid input data."));
        
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
            return NotFound(ApiResponse<string>.Fail($"Customer with ID {id} not found."));
        
        entity.FirstName = dto.FirstName;
        entity.LastName = dto.LastName;
        entity.Email = dto.Email;
        entity.PhoneNumber = dto.PhoneNumber;
        
        await _repository.UpdateAsync(entity);
        return Ok(ApiResponse<string>.Ok($"Customer with ID {id} updated successfully."));
    }
    
    // DELETE: api/Customers/5
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
    {
        var exists = await _repository.ExistsAsync(id);
        if (!exists) 
            return NotFound(ApiResponse<string>.Fail($"Customer with ID {id} not found."));
        
        await _repository.DeleteAsync(id);
        return Ok(ApiResponse<string>.Ok($"Customer with ID {id} deleted successfully."));
    }
    
    // Helper method to map Customer to CustomerResponseDto
    private static CustomerResponseDto MapToResponse(Customer c) => new()
    {
        Id = c.Id,
        FirstName = c.FirstName,
        LastName = c.LastName,
        Email = c.Email,
        PhoneNumber = c.PhoneNumber
    };
}