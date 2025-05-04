using System.Reflection;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Doerly.Messaging;

public static class ServiceCollectionExtensions
{
    public static void RegisterEventConsumers(this IServiceCollection services, Assembly assembly)
    {
        services.AddMassTransit(x => { x.AddConsumers(assembly); });
    }
}
