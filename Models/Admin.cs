using System.ComponentModel.DataAnnotations;

namespace EasyDine.Models;

public class Admin
{
    [Key]
    public int Id { get; set; }
    
    [Required, MaxLength(50)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    
}