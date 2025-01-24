using Doerly.Domain.Extensions;
using Doerly.Module.Authorization.DataAccess;
using Doerly.Api.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Doerly.Module.Authorization.Api;

public class ModuleInitializer : IModuleInitializer
{
    public void ConfigureServices(IHostApplicationBuilder builder, IMvcBuilder mvcBuilder)
    {
        builder.Services.AddDbContext<AuthorizationDbContext>();
        builder.Services.RegisterHandlers(typeof(Domain.IAssemblyMarker).Assembly);
        builder.Services.AddLocalization(x => x.ResourcesPath = "Resources");
        mvcBuilder.AddDataAnnotationsLocalization(options =>
        {
            options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(Localization.Resources));
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AuthorizationDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
