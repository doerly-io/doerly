using Doerly.Domain.Extensions;
using Doerly.Module.Catalog.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Doerly.DataAccess.Utils;
using Doerly.Infrastructure.Api;
using Doerly.Module.Catalog.Api.ModuleWrapper;
using Doerly.Proxy.Catalog;

namespace Doerly.Module.Catalog.Api
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void ConfigureServices(IHostApplicationBuilder builder)
        {
            builder.Services.AddDbContext<CatalogDbContext>();
            builder.Services.RegisterHandlers(typeof(Domain.IAssemblyMarker).Assembly);
            builder.Services.AddScoped<ICatalogModuleWrapper, CatalogModuleWrapper>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ApplicationServices.MigrateDatabase<CatalogDbContext>();
        }
    }
}
