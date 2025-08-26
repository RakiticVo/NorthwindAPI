using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NorthwindApi.Application.Abstractions;

namespace NorthwindApi.Infrastructure.Repository;

public sealed class UnitOfWork(NorthwindContext ctx) : IUnitOfWork, IAsyncDisposable
{
    private IDbContextTransaction? _currentTransaction;
    public DbConnection Connection => ctx.Database.GetDbConnection();
    public DbTransaction? CurrentTransaction => _currentTransaction?.GetDbTransaction();

    public async Task BeginAsync(CancellationToken ct = default)
    {
        if (_currentTransaction != null) return;
        _currentTransaction = await ctx.Database.BeginTransactionAsync(ct);
    }

    public async Task CommitAsync(CancellationToken ct = default)
    {
        if (_currentTransaction == null) return;
        await ctx.SaveChangesAsync(ct);
        await _currentTransaction.CommitAsync(ct);
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }

    public async Task RollbackAsync(CancellationToken ct = default)
    {
        if (_currentTransaction == null) return;
        await _currentTransaction.RollbackAsync(ct);
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }

    public async ValueTask DisposeAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }
}