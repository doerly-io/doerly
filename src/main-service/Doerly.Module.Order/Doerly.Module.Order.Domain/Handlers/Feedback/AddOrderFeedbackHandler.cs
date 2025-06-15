using Doerly.Domain.Handlers;
using Doerly.Domain.Models;
using Doerly.Module.Order.DataTransferObjects.Requests;
using Doerly.Module.Profile.DataTransferObjects.Feedback;
using Doerly.Proxy.Profile;

namespace Doerly.Module.Order.Domain.Handlers;

public class AddOrderFeedbackHandler : BaseHandler
{
    private readonly IProfileModuleProxy _profileModuleProxy;

    public AddOrderFeedbackHandler(IProfileModuleProxy profileModuleProxy)
    {
        _profileModuleProxy = profileModuleProxy;
    }

    public async Task<OperationResult> HandleAsync(int orderId, FeedbackRequest request)
    {
        await _profileModuleProxy.CreateFeedback(orderId, request);

        return OperationResult.Success();
    }
}