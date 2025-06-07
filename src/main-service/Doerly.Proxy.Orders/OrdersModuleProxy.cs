using Doerly.Module.Order.Api.ModuleWrapper;

namespace Doerly.Proxy.Orders;

public class OrdersModuleProxy : IOrdersModuleProxy
{
    private readonly IOrdersModuleWrapper _ordersModuleWrapper;

    public OrdersModuleProxy(IOrdersModuleWrapper ordersModuleWrapper)
    {
        _ordersModuleWrapper = ordersModuleWrapper;
    }

    
    
}