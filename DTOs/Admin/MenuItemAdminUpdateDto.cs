using System.ComponentModel.DataAnnotations;

namespace EasyDine.DTOs.Admin;

public class MenuItemAdminUpdateDto
{
    // Explicit override. Null = clear override and use derived rule.
    public bool? AdminIsPopularOverride { get; set; }

    // Update the threshold for popularity. Null = no change.
    [Range(0, int.MaxValue)] 
    public int? PopularityThreshold { get; set; }
}