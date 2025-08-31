namespace EasyDine.Extensions;

public static class DateTimeExtensions
{
    public static DateTime TruncateToMinute(this DateTime dt)
    {
        return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0, dt.Kind);
    }
}