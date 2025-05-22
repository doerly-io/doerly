using Doerly.Domain.Extensions;
using Doerly.Module.Profile.DataAccess;
using Doerly.Infrastructure.Api;
using Doerly.DataAccess.Utils;
using Doerly.Messaging;
using Doerly.Module.Profile.Contracts.Services;
using Doerly.Module.Profile.Domain;
using Doerly.Module.Profile.Domain.Constants;
using Doerly.Module.Profile.Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Doerly.Module.Profile.Api;

public class ModuleInitializer : IModuleInitializer
{
    public void ConfigureServices(IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ProfileDbContext>();
        builder.Services.AddScoped<IProfileService, ProfileService>();
        builder.Services.RegisterHandlers(typeof(Domain.IAssemblyMarker).Assembly);
        builder.Services.RegisterEventConsumers(typeof(IAssemblyMarker).Assembly);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.ApplicationServices.MigrateDatabase<ProfileDbContext>();
        app.ApplicationServices.AddStorageContainer(AzureStorageConstants.ImagesContainerName);
        app.ApplicationServices.AddStorageContainer(AzureStorageConstants.DocumentsContainerName);
    }
}
