namespace NorthwindApi.Application.Common.DateTimes;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime OffsetNow => DateTime.Now;

    public DateTime OffsetUtcNow => DateTime.UtcNow;
    public DateTime VietnameseTimeNow => TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "SE Asia Standard Time");
}