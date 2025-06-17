using Doerly.Module.Payments.Client.LiqPay.Client;
using Microsoft.Extensions.DependencyInjection;


namespace Doerly.Module.Payments.Client.LiqPay;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLiqPayClient(this IServiceCollection services, Action<LiqPayOptions> configure)
    {
        var options = new LiqPayOptions();
        configure?.Invoke(options);

        services.AddScoped<LiqPayClient>(provider => new LiqPayClient(options.PublicKey, options.PrivateKey, provider.GetService<LiqPayHttpClient>()));

        //services.AddHttpClient<LiqPayHttpClient>(client => { });

        services.AddScoped<LiqPayHttpClient>();

        return services;
    }
}
