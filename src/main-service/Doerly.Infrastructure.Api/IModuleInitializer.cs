using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Doerly.Infrastructure.Api;

public interface IModuleInitializer
{
    void ConfigureServices(IHostApplicationBuilder builder);

    void Configure(IApplicationBuilder app, IWebHostEnvironment env);
}
