// DateTimeExtensions.cs
namespace BouvetBackend.Extensions;

public static class DateTimeExtensions
{
    public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
    {
        int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
        return dt.Date.AddDays(-1 * diff).Date;
    }
}
