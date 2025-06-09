using Doerly.Domain.Handlers;
using Doerly.Proxy.Orders;

namespace Doerly.Module.Profile.Domain.Handlers;

public class SelectUserFeedbacksHandler : BaseHandler
{
    private readonly IOrdersModuleProxy _ordersModuleProxy;

    public SelectUserFeedbacksHandler(IOrdersModuleProxy ordersModuleProxy )
    {
        _ordersModuleProxy = ordersModuleProxy;
    }
    
}
