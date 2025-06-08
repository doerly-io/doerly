using System.Reflection;
using Azure.Storage.Blobs;
using Doerly.Domain.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Doerly.Domain.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterHandlers(this IServiceCollection services, Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes()
            .Where(t => typeof(IHandler).IsAssignableFrom(t) && t is { IsAbstract: false, IsInterface: false });

        foreach (var handlerType in handlerTypes)
            services.AddTransient(handlerType);
    }

    public static void AddStorageContainer(this IServiceProvider services, string containerName)
    {
        var blobServiceClient = services.GetRequiredService<BlobServiceClient>();
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        containerClient.CreateIfNotExists();
    }
}
