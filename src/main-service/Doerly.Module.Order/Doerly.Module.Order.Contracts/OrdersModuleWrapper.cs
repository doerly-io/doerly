using Doerly.Domain.Factories;

namespace Doerly.Module.Order.Contracts;

public class OrdersModuleWrapper : IOrdersModuleWrapper
{
    private readonly IHandlerFactory _handlerFactory;

    public OrdersModuleWrapper(IHandlerFactory handlerFactory)
    {
        _handlerFactory = handlerFactory;
    }
    
}
