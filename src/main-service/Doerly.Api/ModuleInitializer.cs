using Doerly.DataAccess;
using Doerly.DataAccess.Utils;
using Doerly.Infrastructure.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Doerly.Api;

public class ModuleInitializer : IModuleInitializer
{
    public void ConfigureServices(IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AddressDbContext>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.ApplicationServices.MigrateDatabase<AddressDbContext>();
    }
}
