namespace NorthwindApi.Domain.Events;

public class EntityCreatedEvent<T>(T entity, DateTimeOffset eventDateTime) : IDomainEvent where T : class
{
    public T Entity { get; } = entity;

    public DateTimeOffset EventDateTime { get; } = eventDateTime;
}
