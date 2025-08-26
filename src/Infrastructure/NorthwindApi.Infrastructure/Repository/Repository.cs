using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NorthwindApi.Application.Abstractions;

namespace NorthwindApi.Infrastructure.Repository;

public class Repository<T>(NorthwindContext ctx) : IRepository<T> where T : class
{
    public async Task<T?> GetByIdAsync(object id, CancellationToken ct = default)
        => await ctx.Set<T>().FindAsync([id], ct);

    public async Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>>? predicate, CancellationToken ct = default)
    {
        var q = ctx.Set<T>().AsNoTracking();
        if (predicate != null) q = q.Where(predicate);
        return await q.ToListAsync(ct);
    }

    public async Task AddAsync(T entity, CancellationToken ct = default)
        => await ctx.Set<T>().AddAsync(entity, ct);

    public void Update(T entity) => ctx.Set<T>().Update(entity);

    public void Remove(T entity) => ctx.Set<T>().Remove(entity);
}