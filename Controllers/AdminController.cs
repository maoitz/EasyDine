using EasyDine.Data;
using EasyDine.DTOs;
using EasyDine.DTOs.Admin;
using EasyDine.Models;
using EasyDine.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyDine.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly AuthService _authService;
    
    public AdminController(AppDbContext context, AuthService authService)
    {
        _context = context;
        _authService = authService;
    }
    
    // POST: api/admins
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<string>), 201)]
    [ProducesResponseType(typeof(ApiResponse<string>), 400)]
    [ProducesResponseType(typeof(ApiResponse<string>), 409)]
    public async Task<IActionResult> Create(AdminCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<string>.Fail("Invalid input"));
        
        var exists = await _context.Admins.AnyAsync(a => a.Username == dto.Username);
        if (exists)
            return Conflict(ApiResponse<string>.Fail("Username is already taken"));
        
        var admin = new Admin { Username = dto.Username };
        admin.PasswordHash = _authService.HashPassword(admin, dto.Password);
        
        _context.Admins.Add(admin);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetById), new { id = admin.Id },
            ApiResponse<string>.Ok($"Admin '{admin.Username}' created."));
    }
    
    // GET: api/admins/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> GetById(int id)
    {
        var admin = await _context.Admins.AsNoTracking()
            .Select(a => new { a.Id, a.Username })
            .FirstOrDefaultAsync(a => a.Id == id);
        
        if (admin is null)
            return NotFound(ApiResponse<object>.Fail($"Admin with ID {id} not found."));
        
        return Ok(ApiResponse<object>.Ok(admin, "Admin retrieved successfully."));
    }
    
    // PATCH: api/admins/5/password
    [HttpPatch("{id:int}/password")]
    [ProducesResponseType(typeof(ApiResponse<string>), 200)]
    [ProducesResponseType(typeof(ApiResponse<string>), 404)]
    public async Task<IActionResult> ChangePassword(int id, AdminChangePasswordDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<string>.Fail("Invalid input"));
        
        var admin = await _context.Admins.FindAsync(id);
        if (admin is null)
            return NotFound(ApiResponse<string>.Fail($"Admin with ID {id} not found."));
        
        admin.PasswordHash = _authService.HashPassword(admin, dto.NewPassword);
        await _context.SaveChangesAsync();
        
        return Ok(ApiResponse<string>.Ok("Password updated successfully."));
    }
}