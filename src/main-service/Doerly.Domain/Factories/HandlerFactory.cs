using Doerly.Domain.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Doerly.Domain.Factories;

public class HandlerFactory : IHandlerFactory
{
    private readonly IServiceProvider _serviceProvider;

    public HandlerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public THandler CreateHandler<THandler>() where THandler : IHandler
    {
        return _serviceProvider.GetRequiredService<THandler>();
    }
}
