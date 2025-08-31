namespace EasyDine.DTOs.Availability;

public class TableAvailabilityDto
{
    public int Id { get; set; }
    public int Number { get; set; }
    public int Seats { get; set; }
    
}

public class TableAvailabilityDetailDto : TableAvailabilityDto
{
    public bool IsAvailable { get; set; }
    public DateTime? NextAvailableAt { get; set; } // null if available now
}