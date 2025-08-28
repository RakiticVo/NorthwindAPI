namespace NorthwindApi.Application.Common.DateTimes;

public interface IDateTimeProvider
{
    DateTime OffsetNow { get; }

    DateTime OffsetUtcNow { get; }

    DateTime VietnameseTimeNow { get; }
}