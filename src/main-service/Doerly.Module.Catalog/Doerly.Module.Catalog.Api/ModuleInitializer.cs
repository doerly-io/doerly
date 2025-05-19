using Doerly.Api.Infrastructure;
using Doerly.Domain.Extensions;
using Doerly.Module.Catalog.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Doerly.DataAccess.Utils;

namespace Doerly.Module.Catalog.Api
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void ConfigureServices(IHostApplicationBuilder builder)
        {
            builder.Services.AddDbContext<CatalogDbContext>();
            builder.Services.RegisterHandlers(typeof(Domain.IAssemblyMarker).Assembly);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ApplicationServices.MigrateDatabase<CatalogDbContext>();
        }
    }
}
