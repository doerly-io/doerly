using Doerly.Domain.Extensions;
using Doerly.Module.Profile.DataAccess;
using Doerly.Api.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Doerly.Module.Profile.Api;

public class ModuleInitializer : IModuleInitializer
{
    public void ConfigureServices(IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ProfileDbContext>();
        builder.Services.RegisterHandlers(typeof(Domain.IAssemblyMarker).Assembly);
        builder.Services.AddLocalization(x => x.ResourcesPath = "Doerly.Module.Profile.Localization");
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ProfileDbContext>();
        dbContext.Database.Migrate();
    }
}