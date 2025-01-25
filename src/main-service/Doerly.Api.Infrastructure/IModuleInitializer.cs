using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Doerly.Api.Infrastructure;

public interface IModuleInitializer
{
    void ConfigureServices(IHostApplicationBuilder builder, IMvcBuilder mvcBuilder);

    void Configure(IApplicationBuilder app, IWebHostEnvironment env);
}
