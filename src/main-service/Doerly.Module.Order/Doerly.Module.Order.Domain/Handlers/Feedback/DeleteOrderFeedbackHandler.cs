using Doerly.Domain.Handlers;
using Doerly.Proxy.Profile;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Order.Domain.Handlers;

public class DeleteOrderFeedbackHandler : BaseHandler
{
    private readonly IProfileModuleProxy _profileModuleProxy;

    public DeleteOrderFeedbackHandler(IProfileModuleProxy profileModuleProxy)
    {
        _profileModuleProxy = profileModuleProxy;
    }

    public async Task HandleAsync(int feedbackId)
    {
        await _profileModuleProxy.DeleteFeedback(feedbackId);
    }
}