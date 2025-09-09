using NorthwindApi.Domain.Entities;

namespace NorthwindApi.Application.Common;

public interface ICrudService<T, in TKey> where T : BaseEntity<TKey>
{
    Task<List<T>> GetAsync();
    
    Task<List<T>> GetPagedAsync(int pageNumber, int pageSize, Func<IQueryable<T>, IQueryable<T>>? include = null);
    
    Task<T?> GetByIdAsync(TKey id);
    
    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
}