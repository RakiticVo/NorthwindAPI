using System.Runtime.InteropServices;

namespace NorthwindApi.Application.Common.DateTimes;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime OffsetNow => DateTime.Now;

    public DateTime OffsetUtcNow => DateTime.UtcNow;

    private const string WindowsTimeZoneId = "SE Asia Standard Time";
    private const string LinuxTimeZoneId = "Asia/Ho_Chi_Minh";

    private static TimeZoneInfo TimeZone =>
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? TimeZoneInfo.FindSystemTimeZoneById(WindowsTimeZoneId)
            : TimeZoneInfo.FindSystemTimeZoneById(LinuxTimeZoneId);

    public DateTime VietnameseTimeNow => 
        TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZone);
}