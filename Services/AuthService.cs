using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EasyDine.Data;
using EasyDine.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace EasyDine.Services;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly PasswordHasher<Admin> _passwordHasher = new();
    
    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<string?> LoginAsync(string username, string password)
    {
        var admin = await _context.Admins.AsNoTracking().FirstOrDefaultAsync(a => a.Username == username);
        if (admin is null) return null;
        
        var result = _passwordHasher.VerifyHashedPassword(admin, admin.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed) return null;
        
        return GenerateJwt(admin);
    }
    
    public string HashPassword(Admin admin, string plain) =>
        _passwordHasher.HashPassword(admin, plain);

    private string GenerateJwt(Admin admin)
    {
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["jwt:key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, admin.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, admin.Username),
            new Claim(ClaimTypes.Role, "Admin")
        };
        
        var token = new JwtSecurityToken(
            issuer: _configuration["jwt:issuer"],
            audience: _configuration["jwt:audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(12),
            signingCredentials: creds
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}