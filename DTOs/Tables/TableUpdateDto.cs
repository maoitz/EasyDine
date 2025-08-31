using System.ComponentModel.DataAnnotations;

namespace EasyDine.DTOs.Tables;

public class TableUpdateDto
{
    [Required]
    public int Number { get; set; }
    
    [Required]
    public int Seats { get; set; }
}