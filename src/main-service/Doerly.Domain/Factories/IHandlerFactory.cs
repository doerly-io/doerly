using Doerly.Domain.Handlers;

namespace Doerly.Domain.Factories;

public interface IHandlerFactory : IFactory
{
    THandler CreateHandler<THandler>() where THandler : IHandler;
}
