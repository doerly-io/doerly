using Microsoft.Extensions.DependencyInjection;

namespace Doerly.Proxy.BaseProxy;

public static class ProxyInitializer
{
    public static IServiceCollection AddProxy<TInterface, TImplementation>(this IServiceCollection services)
        where TImplementation : class, TInterface, IModuleProxy
        where TInterface : class
    {
        services.AddScoped<TInterface, TImplementation>();
        return services;
    }
}
