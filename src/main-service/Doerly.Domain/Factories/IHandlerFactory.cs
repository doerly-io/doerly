using Doerly.Domain.Handlers;

namespace Doerly.Domain.Factories;

public interface IHandlerFactory : IFactory
{
    THandler Get<THandler>() where THandler : IHandler;
}
