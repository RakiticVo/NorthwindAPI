namespace NorthwindApi.Application.Abstractions;

/// <summary>
/// Distributed lock (ở đây dùng SQL Server sp_getapplock).
/// </summary>
public interface IDistributedLock
{
    /// <summary>
    /// Cố gắng lấy lock theo resourceKey. Trả về true nếu lấy được.
    /// Lock tồn tại trong phạm vi transaction đang mở của UnitOfWork.
    /// </summary>
    Task<bool> TryAcquireAsync(string resourceKey, TimeSpan timeout, CancellationToken ct = default);

    /// <summary>
    /// Giải phóng lock cho resourceKey (nếu đã giữ).
    /// </summary>
    Task ReleaseAsync(string resourceKey, CancellationToken ct = default);
}