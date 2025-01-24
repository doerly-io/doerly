using System.Reflection;
using Doerly.Domain.Handlers;
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
}