using System.Data;
using Microsoft.Data.SqlClient;
using NorthwindApi.Application.Abstractions;

namespace NorthwindApi.Infrastructure.Locking;

public sealed class SqlDistributedLock(IUnitOfWork uow) : IDistributedLock
{
    private const string LockOwner = "Transaction";
    private const string LockMode = "Exclusive";

    public async Task<bool> TryAcquireAsync(string resourceKey, TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(resourceKey);
        if (uow.CurrentTransaction is null)
            throw new InvalidOperationException("A transaction must be active. Call BeginTransactionAsync first.");

        using var command = CreateCommand("sp_getapplock", new[]
        {
            new SqlParameter("@Resource", resourceKey),
            new SqlParameter("@LockMode", LockMode),
            new SqlParameter("@LockOwner", LockOwner),
            new SqlParameter("@LockTimeout", (int)timeout.TotalMilliseconds)
        });

        var returnParam = new SqlParameter("@Result", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        command.Parameters.Add(returnParam);

        await command.ExecuteNonQueryAsync(cancellationToken);
        return (int)(returnParam.Value ?? -999) >= 0;
    }

    public async Task ReleaseAsync(string resourceKey, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(resourceKey);
        if (uow.CurrentTransaction is null)
            return;

        using var command = CreateCommand("sp_releaseapplock", new[]
        {
            new SqlParameter("@Resource", resourceKey),
            new SqlParameter("@LockOwner", LockOwner)
        });

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private SqlCommand CreateCommand(string procedureName, SqlParameter[] parameters)
    {
        var command = (SqlCommand)uow.Connection.CreateCommand();
        command.Transaction = uow.CurrentTransaction as SqlTransaction;
        command.CommandText = procedureName;
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddRange(parameters);
        return command;
    }
}