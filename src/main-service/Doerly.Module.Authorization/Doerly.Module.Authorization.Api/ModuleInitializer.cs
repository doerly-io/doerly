using Doerly.Domain.Extensions;
using Doerly.Module.Authorization.DataAccess;
using Doerly.Infrastructure.Api;
using Doerly.DataAccess.Utils;
using Doerly.Messaging;
using Doerly.Module.Authorization.Contracts;
using Doerly.Module.Authorization.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Doerly.Module.Authorization.Api;

public class ModuleInitializer : IModuleInitializer
{
    public void ConfigureServices(IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AuthorizationDbContext>();
        builder.Services.RegisterHandlers(typeof(IAssemblyMarker).Assembly);
        builder.Services.RegisterEventConsumers(typeof(IAssemblyMarker).Assembly);
        builder.Services.AddScoped<IAuthorizationModuleWrapper, AuthorizationModuleWrapper>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.ApplicationServices.MigrateDatabase<AuthorizationDbContext>();
    }
}
