using Doerly.Api.Infrastructure;
using Doerly.Domain.Extensions;
using Doerly.Module.Payments.Client.LiqPay;
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
        var configuration = builder.Services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        var paymentSettingsConfig = configuration.GetSection(PaymentSettings.PaymentSettingsName);
        builder.Services.AddOptions<PaymentSettings>()
            .Bind(paymentSettingsConfig)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var paymentSettings = paymentSettingsConfig.Get<PaymentSettings>();


        builder.Services.RegisterHandlers(typeof(Domain.IAssemblyMarker).Assembly);
       
        builder.Services.AddLiqPayClient(options =>
        {
            var liqPaySettings = paymentSettings.LiqPay;
            
            options.PublicKey = liqPaySettings.PublicKey;
            options.PrivateKey = liqPaySettings.PrivateKey;
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        //app.ApplicationServices.MigrateDatabase<PaymentDbContext>();
    }
}
