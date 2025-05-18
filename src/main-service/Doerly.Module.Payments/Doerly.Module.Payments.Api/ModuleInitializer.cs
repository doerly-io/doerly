using Doerly.DataAccess.Utils;
using Doerly.Infrastructure.Api;
using Doerly.Domain.Extensions;
using Doerly.Module.Payments.Api.ModuleWrapper;
using Doerly.Module.Payments.BaseClient;
using Doerly.Module.Payments.Client.LiqPay;
using Doerly.Module.Payments.DataAccess;
using Doerly.Module.Payments.Domain.Adapters;
using Doerly.Module.Payments.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Doerly.Module.Payments.Api;

public class ModuleInitializer : IModuleInitializer
{
    public void ConfigureServices(IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<PaymentDbContext>();
        builder.Services.RegisterHandlers(typeof(Domain.IAssemblyMarker).Assembly);
        builder.Services.AddScoped<IPaymentModuleWrapper, PaymentModuleWrapper>();
        
        var configuration = builder.Services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        var paymentSettingsConfig = configuration.GetSection(PaymentSettings.PaymentSettingsName);
        builder.Services.AddOptions<PaymentSettings>()
            .Bind(paymentSettingsConfig)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var paymentSettings = paymentSettingsConfig.Get<PaymentSettings>();
        builder.Services.AddLiqPayClient(options =>
        {
            var liqPaySettings = paymentSettings.LiqPay;
            options.PublicKey = liqPaySettings.PublicKey;
            options.PrivateKey = liqPaySettings.PrivateKey;
        });

        builder.Services.AddScoped<IPaymentModuleWrapper, PaymentModuleWrapper>();
        builder.Services.AddTransient<LiqPayPaymentAdapter>();

        builder.Services.AddScoped<PaymentClientFactory>(sp => aggregator =>
        {
            return aggregator switch
            {
                EPaymentAggregator.LiqPay => sp.GetRequiredService<LiqPayClient>(),
                _ => throw new ArgumentOutOfRangeException(nameof(aggregator), aggregator, null)
            };
        });

        builder.Services.AddScoped<PaymentAdapterFactory>(sp => aggregator =>
        {
            return aggregator switch
            {
                EPaymentAggregator.LiqPay => sp.GetRequiredService<LiqPayPaymentAdapter>(),
                _ => throw new ArgumentOutOfRangeException(nameof(aggregator), aggregator, null)
            };
        });

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.ApplicationServices.MigrateDatabase<PaymentDbContext>();
    }
}
