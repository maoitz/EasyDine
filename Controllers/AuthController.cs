using System.ComponentModel.DataAnnotations;
using EasyDine.DTOs;
using EasyDine.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyDine.Controllers;

public class LoginDto
{
    [Required] public string Username { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    public AuthController(AuthService authService) => _authService = authService;

    // POST: api/auth/login
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<string>), 200)]
    [ProducesResponseType(typeof(ApiResponse<string>), 401)]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<string>.Fail("Invalid login payload."));
        
        var token = await _authService.LoginAsync(loginDto.Username, loginDto.Password);
        if (token is null)
            return Unauthorized(ApiResponse<string>.Fail("Invalid username or password."));
        
        return Ok(ApiResponse<string>.Ok(token, "Login successful."));
    }
}