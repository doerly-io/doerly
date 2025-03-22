using Doerly.Api.Infrastructure;
using Doerly.DataAccess.Utils;
using Doerly.Domain;
using Doerly.Domain.Extensions;
using Doerly.Module.Communication.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Doerly.Module.Communication.Api;

public class ModuleInitializer : IModuleInitializer
{
    public void ConfigureServices(IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<CommunicationDbContext>();
        builder.Services.RegisterHandlers(typeof(Domain.IAssemblyMarker).Assembly);
        builder.Services.RegisterEventConsumers(typeof(IAssemblyMarker).Assembly);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.ApplicationServices.MigrateDatabase<CommunicationDbContext>();
    }
}