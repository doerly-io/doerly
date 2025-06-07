using Doerly.Domain.Extensions;
using Doerly.Infrastructure.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Doerly.Module.Statistics.Api;

public class ModuleInitializer : IModuleInitializer
{
    public void ConfigureServices(IHostApplicationBuilder builder)
    {
        builder.Services.RegisterHandlers(typeof(Domain.IAssemblyMarker).Assembly);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        
    }
}
