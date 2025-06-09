using Doerly.Module.Order.Contracts;

namespace Doerly.Proxy.Orders;

public class OrdersModuleProxy : IOrdersModuleProxy
{
    private readonly IOrdersModuleWrapper _ordersModuleWrapper;

    public OrdersModuleProxy(IOrdersModuleWrapper ordersModuleWrapper)
    {
        _ordersModuleWrapper = ordersModuleWrapper;
    }

    
    
}
