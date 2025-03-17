using System.Reflection;
using Doerly.Domain.Handlers;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Doerly.Domain.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterHandlers(this IServiceCollection services, Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes()
            .Where(t => typeof(IHandler).IsAssignableFrom(t) && t is { IsAbstract: false, IsInterface: false });

        foreach (var handlerType in handlerTypes)
            services.AddTransient(handlerType);
    }

    public static void RegisterEventConsumers(this IServiceCollection services, Assembly assembly)
    {
        services.AddMassTransit(x => { x.AddConsumers(assembly); });
    }
}
