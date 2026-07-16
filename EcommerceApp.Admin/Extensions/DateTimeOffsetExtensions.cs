namespace EcommerceApp.Extensions;

public static class DateTimeOffsetExtensions
{
    public static DateTimeOffset TruncateToPostgresPrecision(this DateTimeOffset dto)
    {
        // 1 microsecond = 10 ticks. This clips the 7th .NET decimal digit.
        return dto.AddTicks(-(dto.Ticks % 10));
    }
}