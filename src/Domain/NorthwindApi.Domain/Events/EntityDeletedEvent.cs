namespace NorthwindApi.Domain.Events;

public class EntityDeletedEvent<T>(T entity, DateTimeOffset eventDateTime) : IDomainEvent where T : class
{
    public T Entity { get; } = entity;

    public DateTimeOffset EventDateTime { get; } = eventDateTime;
}
