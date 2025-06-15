using Doerly.Domain.Handlers;
using Doerly.Domain.Models;
using Doerly.Module.Order.DataTransferObjects.Requests;
using Doerly.Module.Profile.DataTransferObjects.Feedback;
using Doerly.Proxy.Profile;

namespace Doerly.Module.Order.Domain.Handlers;

public class UpdatedOrderFeedbackHandler : BaseHandler
{
    private readonly IProfileModuleProxy _profileModuleProxy;

    public UpdatedOrderFeedbackHandler(IProfileModuleProxy profileModuleProxy)
    {
        _profileModuleProxy = profileModuleProxy;
    }

    public async Task<OperationResult> HandleAsync(int feedbackId, FeedbackRequest request)
    {
        await _profileModuleProxy.UpdateFeedback(feedbackId, request);

        return OperationResult.Success();
    }
}