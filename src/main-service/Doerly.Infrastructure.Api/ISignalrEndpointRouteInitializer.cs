using Microsoft.AspNetCore.Routing;

namespace Doerly.Infrastructure.Api;

public interface ISignalrEndpointRouteInitializer
{
    void ConfigureEndpoints(IEndpointRouteBuilder endpoints);
}