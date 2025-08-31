namespace EasyDine.Services;

public class BookingRulesOptions
{
    // "hh:mm" format, e.g. "11:00"
    public TimeSpan Opening { get; set; } = new (16, 0, 0);
    public TimeSpan Closing { get; set; } = new (22, 0, 0);
    public int MinDurationMinutes { get; set; } = 30;
    public int MaxDurationMinutes { get; set; } = 240;
}