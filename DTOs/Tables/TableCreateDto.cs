using System.ComponentModel.DataAnnotations;

namespace EasyDine.DTOs.Tables;

public class TableCreateDto
{
    [Required]
    public int Number { get; set; }
    
    [Required]
    public int Seats { get; set; }
}