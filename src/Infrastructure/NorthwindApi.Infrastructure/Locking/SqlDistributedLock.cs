using System.Data;
using Microsoft.Data.SqlClient;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Infrastructure.Repository;

namespace NorthwindApi.Infrastructure.Locking;

public sealed class SqlDistributedLock(UnitOfWork uow) : IDistributedLock
{
    public async Task<bool> TryAcquireAsync(string resourceKey, TimeSpan timeout, CancellationToken ct = default)
    {
        if (uow.CurrentTransaction is null)
            throw new InvalidOperationException("Lock requires an active transaction. Call UnitOfWork.BeginAsync first.");

        var cmd = uow.Connection.CreateCommand();
        cmd.Transaction = uow.CurrentTransaction;
        cmd.CommandText = "sp_getapplock";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add(new SqlParameter("@Resource", resourceKey));
        cmd.Parameters.Add(new SqlParameter("@LockMode", "Exclusive"));
        cmd.Parameters.Add(new SqlParameter("@LockOwner", "Transaction")); // lock gắn với transaction
        cmd.Parameters.Add(new SqlParameter("@LockTimeout", (int)timeout.TotalMilliseconds));

        var returnParam = new SqlParameter("@Result", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(returnParam);

        await cmd.ExecuteNonQueryAsync(ct);
        var result = (int)(returnParam.Value ?? -999);

        // 0, 1: giữ được lock (0=đang giữ, 1=chuyển mode); -1: timeout; -2: deadlock; -3: param sai; -999: lỗi khác
        return result >= 0;
    }

    public async Task ReleaseAsync(string resourceKey, CancellationToken ct = default)
    {
        if (uow.CurrentTransaction is null) return;

        var cmd = uow.Connection.CreateCommand();
        cmd.Transaction = uow.CurrentTransaction;
        cmd.CommandText = "sp_releaseapplock";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add(new SqlParameter("@Resource", resourceKey));
        cmd.Parameters.Add(new SqlParameter("@LockOwner", "Transaction"));

        await cmd.ExecuteNonQueryAsync(ct);
    }
}