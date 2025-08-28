using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.Common.Queries;

namespace NorthwindApi.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHandlers(this IServiceCollection services, Assembly assembly)
    {
        // Đăng ký IQueryHandler
        var queryHandlers = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)))
            .ToList();

        foreach (var handler in queryHandlers)
        {
            var interfaceType = handler.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));
            services.AddScoped(interfaceType, handler);
        }

        // Đăng ký ICommandHandler
        var commandHandlers = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)))
            .ToList();

        foreach (var handler in commandHandlers)
        {
            var interfaceType = handler.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>));
            services.AddScoped(interfaceType, handler);
        }

        return services;
    }
}