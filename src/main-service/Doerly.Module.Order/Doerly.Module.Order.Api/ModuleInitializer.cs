using Doerly.Infrastructure.Api;
using Doerly.Domain.Extensions;
using Doerly.Module.Order.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Doerly.DataAccess.Utils;
using Doerly.Module.Order.Domain;
using Doerly.Messaging;
using Doerly.Module.Order.Contracts;

namespace Doerly.Module.Order.Api;

public class ModuleInitializer : IModuleInitializer
{
    public void ConfigureServices(IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<OrderDbContext>();
        builder.Services.RegisterHandlers(typeof(IAssemblyMarker).Assembly);
        builder.Services.RegisterEventConsumers(typeof(IAssemblyMarker).Assembly);
        builder.Services.AddScoped<IOrdersModuleWrapper, OrdersModuleWrapper>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.ApplicationServices.MigrateDatabase<OrderDbContext>();
    }
}
