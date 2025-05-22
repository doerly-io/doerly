using Microsoft.AspNetCore.Routing;

namespace Doerly.Infrastructure.Api;

public interface IEndpointRouteInitializer
{
    void ConfigureEndpoints(IEndpointRouteBuilder endpoints);
}