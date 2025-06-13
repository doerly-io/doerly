using Doerly.Common.Settings.Constants;
using Doerly.DataAccess.Utils;
using Doerly.Domain.Extensions;
using Doerly.Infrastructure.Api;
using Doerly.Messaging;
using Doerly.Module.Notification.Api.Hubs;
using Doerly.Module.Notification.DataAccess;
using Doerly.Module.Notification.Domain;
using Doerly.Module.Notification.Proxy;
using Doerly.Module.Notification.Domain.Services;

namespace Doerly.Module.Notification.Api;

public class ModuleInitializer: IModuleInitializer, ISignalrEndpointRouteInitializer
{
    public void ConfigureServices(IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<NotificationDbContext>();
        builder.Services.RegisterHandlers(typeof(IAssemblyMarker).Assembly);
        builder.Services.RegisterEventConsumers(typeof(IAssemblyMarker).Assembly);
        builder.Services.AddSingleton<ISignalrEndpointRouteInitializer, ModuleInitializer>();
        builder.Services.AddScoped<INotificationModuleProxy, NotificationModuleProxy>();
        builder.Services.AddScoped<INotificationSender, NotificationHubSender>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.ApplicationServices.MigrateDatabase<NotificationDbContext>();
    }
    
    public void ConfigureEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHub<NotificationHub>(HubConstants.notificationHub).RequireAuthorization();
    }
}