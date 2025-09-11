using Microsoft.EntityFrameworkCore;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Validator;
using NorthwindApi.Domain.Entities;
using NorthwindApi.Domain.Events;

namespace NorthwindApi.Application.Common;

public class CrudService<T, TKey>(Dispatcher dispatcher, IRepository<T, TKey> repository)
    : ICrudService<T, TKey> 
    where T : BaseEntity<TKey>
{
    private readonly IUnitOfWork _unitOfWork = repository.UnitOfWork;

    public Task<List<T>> GetAsync() => repository.ToListAsync(repository.GetQueryableSet());
    public async Task<List<T>> GetPagedAsync(int pageNumber, int pageSize, Func<IQueryable<T>, IQueryable<T>>? include = null)
    {
        var query = repository.GetQueryableSet();
        if (include != null)
        {
            query = include(query);
        }

        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<T?> GetByIdAsync(TKey id)
    {
        ValidationExceptions.Requires(id != null, "Invalid Id");
        return await repository.FirstOrDefaultAsync(repository.GetQueryableSet().Where(x => x.Id != null && x.Id.Equals(id)));
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await repository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await dispatcher.DispatchAsync(new EntityCreatedEvent<T>(entity, DateTimeOffset.UtcNow), cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await repository.UpdateAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await dispatcher.DispatchAsync(new EntityUpdatedEvent<T>(entity, DateTimeOffset.UtcNow), cancellationToken);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        repository.Remove(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await dispatcher.DispatchAsync(new EntityDeletedEvent<T>(entity, DateTimeOffset.UtcNow), cancellationToken);
    }
}