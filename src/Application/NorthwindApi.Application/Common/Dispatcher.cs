using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.Common.Queries;
using NorthwindApi.Domain.Events;

namespace NorthwindApi.Application.Common;

public class Dispatcher(IServiceProvider serviceProvider)
{
    private readonly List<Type> _eventHandlers = [];

    public void RegisterEventHandlers(Assembly assembly, IServiceCollection services)
    {
        var types = assembly.GetTypes()
            .Where(x => x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)))
            .ToList();

        foreach (var type in types)
        {
            services.AddTransient(type);
            _eventHandlers.Add(type);
        }
    }

    public async Task<T> DispatchAsync<T>(IQuery<T> query, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(T));
        var handler = serviceProvider.GetService(handlerType)
            ?? throw new InvalidOperationException($"No handler registered for {query.GetType().Name}");
        return await ((dynamic)handler).HandleAsync((dynamic)query, cancellationToken);
    }

    public async Task<T> DispatchAsync<T>(ICommand<T> command, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(T));
        var handler = serviceProvider.GetService(handlerType)
            ?? throw new InvalidOperationException($"No handler registered for {command.GetType().Name}");
        return await ((dynamic)handler).HandleAsync((dynamic)command, cancellationToken);
    }

    public async Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        foreach (var handlerType in _eventHandlers)
        {
            var canHandleEvent = handlerType.GetInterfaces()
                .Any(x => x.IsGenericType
                          && x.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)
                          && x.GenericTypeArguments.Length > 0
                          && x.GenericTypeArguments[0] == domainEvent.GetType());

            if (canHandleEvent)
            {
                var handler = serviceProvider.GetService(handlerType)
                    ?? throw new InvalidOperationException($"No handler registered for {domainEvent.GetType().Name}");
                await ((dynamic)handler).HandleAsync((dynamic)domainEvent, cancellationToken);
            }
        }
    }
}