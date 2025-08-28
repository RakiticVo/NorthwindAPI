using System.Linq.Expressions;
using NorthwindApi.Domain.Entities;

namespace NorthwindApi.Application.Abstractions;

public interface IRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    IUnitOfWork UnitOfWork { get; }
    IQueryable<TEntity> GetQueryableSet();

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    void Remove(TEntity entity);
    
    Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query);

    Task<T?> SingleOrDefaultAsync<T>(IQueryable<T> query);

    Task<List<T>> ToListAsync<T>(IQueryable<T> query);
}