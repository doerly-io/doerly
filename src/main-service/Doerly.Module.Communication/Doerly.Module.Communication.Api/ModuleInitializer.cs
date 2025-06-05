using Doerly.DataAccess.Utils;
using Doerly.Domain.Extensions;
using Doerly.Infrastructure.Api;
using Doerly.Module.Communication.DataAccess;
using Doerly.Module.Communication.Domain;
using Doerly.Module.Communication.Domain.Hubs;
using Doerly.Messaging;
using Doerly.Module.Communication.Domain.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Doerly.Module.Communication.Api;

public class ModuleInitializer : IModuleInitializer, IEndpointRouteInitializer
{
    public void ConfigureServices(IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<CommunicationDbContext>();
        builder.Services.RegisterHandlers(typeof(IAssemblyMarker).Assembly);
        builder.Services.RegisterEventConsumers(typeof(IAssemblyMarker).Assembly);
        builder.Services.AddSingleton<IEndpointRouteInitializer, ModuleInitializer>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.ApplicationServices.MigrateDatabase<CommunicationDbContext>();
        app.ApplicationServices.AddStorageContainer(CommunicationConstants.AzureStorage.FilesContainerName);
    }

    public void ConfigureEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHub<CommunicationHub>("/communicationhub").RequireAuthorization();
    }
}