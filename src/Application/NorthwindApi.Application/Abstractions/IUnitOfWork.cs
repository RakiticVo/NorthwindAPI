using System.Data;
using System.Data.Common;

namespace NorthwindApi.Application.Abstractions;

public interface IUnitOfWork : IAsyncDisposable
{
    DbConnection Connection { get; }
    DbTransaction? CurrentTransaction { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<IDisposable> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
        CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}