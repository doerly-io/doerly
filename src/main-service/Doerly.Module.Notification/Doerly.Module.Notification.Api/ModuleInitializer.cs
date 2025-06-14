using Doerly.Common.Settings.Constants;
using Doerly.DataAccess.Utils;
using Doerly.Domain.Extensions;
using Doerly.Infrastructure.Api;
using Doerly.Messaging;
using Doerly.Module.Notification.Api.Hubs;
using Doerly.Module.Notification.DataAccess;
using Doerly.Module.Notification.Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Doerly.Module.Notification.Api;

public class ModuleInitializer: IModuleInitializer, ISignalrEndpointRouteInitializer
{
    public void ConfigureServices(IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<NotificationDbContext>();
        builder.Services.RegisterHandlers(typeof(Doerly.Module.Notification.Domain.IAssemblyMarker).Assembly);
        builder.Services.RegisterEventConsumers(typeof(Doerly.Module.Notification.Messaging.IAssemblyMarker).Assembly);
        builder.Services.AddSingleton<ISignalrEndpointRouteInitializer, ModuleInitializer>();
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