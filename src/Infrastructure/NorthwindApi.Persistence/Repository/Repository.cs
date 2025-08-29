using Microsoft.EntityFrameworkCore;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common.DateTimes;
using NorthwindApi.Domain.Entities;

namespace NorthwindApi.Persistence.Repository;

public class Repository<TEntity, TKey>(IDateTimeProvider dateTimeProvider, NorthwindContext dbContext)
    : IRepository<TEntity, TKey>
    where TEntity : BaseEntity<TKey>
{
    private DbSet<TEntity> DbSet => dbContext.Set<TEntity>();
    public IUnitOfWork UnitOfWork => dbContext;

    public IQueryable<TEntity> GetQueryableSet()
    {
        var query = DbSet.AsQueryable();
        return dbContext.Model.FindEntityType(typeof(TEntity))
            ?.GetNavigations().Aggregate(query, func: (current, property) => current.Include
                (property.Name)) ?? query;
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = dateTimeProvider.OffsetNow;
        await DbSet.AddAsync(entity, cancellationToken);
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = dateTimeProvider.OffsetNow;
        DbSet.Update(entity);
        return Task.CompletedTask;
    }

    public void Remove(TEntity entity)
    {
        DbSet.Remove(entity);
    }

    public Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query) => query.FirstOrDefaultAsync();

    public Task<T?> SingleOrDefaultAsync<T>(IQueryable<T> query)=> query.SingleOrDefaultAsync();

    public Task<List<T>> ToListAsync<T>(IQueryable<T> query) => query.ToListAsync();
}