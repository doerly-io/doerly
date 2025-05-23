using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Doerly.Infrastructure.Api;

public static class ModuleRegistrator
{
    public static void RegisterModule(this WebApplicationBuilder builder, IModuleInitializer moduleInitializer)
    {
        builder.Services.AddSingleton(moduleInitializer);
        moduleInitializer.ConfigureServices(builder);
    }
}
