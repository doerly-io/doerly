using Doerly.Domain.Factories;

namespace Doerly.Module.Order.Api.ModuleWrapper;

public interface IOrdersModuleWrapper
{
    
}

public class OrdersModuleWrapper : IOrdersModuleWrapper
{
    private readonly IHandlerFactory _handlerFactory;

    public OrdersModuleWrapper(IHandlerFactory handlerFactory)
    {
        _handlerFactory = handlerFactory;
    }
    
    
    
}