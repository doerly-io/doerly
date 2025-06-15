using Doerly.Domain.Factories;
using Doerly.Domain.Models;
using Doerly.Module.Order.DataTransferObjects.Requests;
using Doerly.Module.Profile.DataTransferObjects;
using Doerly.Module.Profile.DataTransferObjects.Feedback;
using Doerly.Module.Profile.Domain.Handlers;
using Doerly.Module.Profile.Domain.Handlers.Feedback;

namespace Doerly.Proxy.Profile;

public class ProfileModuleProxy : IProfileModuleProxy
{
    private readonly IHandlerFactory _handlerFactory;

    public ProfileModuleProxy(IHandlerFactory handlerFactory)
    {
        _handlerFactory = handlerFactory;
    }

    public Task<OperationResult<ProfileDto>> GetProfileAsync(int userId)
    {
        var profileResponse = _handlerFactory.Get<GetProfileHandler>().HandleAsync(userId);
        return profileResponse;
    }

    public Task<OperationResult<IEnumerable<ProfileDto>>> GetProfilesAsync(int[] userIds)
    {
        var profilesResponse = _handlerFactory.Get<GetProfilesHandler>().HandleAsync(userIds);
        return profilesResponse;
    }

    public Task<IEnumerable<ProfileShortInfoWithAvatarDto>> GetProfilesShortInfoWithAvatarAsync(
        IEnumerable<int> userIds)
    {
        var profilesShortResponse = _handlerFactory.Get<GetProfilesShortInfoWithAvatars>().HandleAsync(userIds);
        return profilesShortResponse;
    }

    public async Task<FeedbackResponse> GetFeedback(int orderId)
    {
        var feedback = await _handlerFactory.Get<FeedbackByOrderIdHandler>().HandleAsync(orderId);
        return feedback;
    }

    public async Task CreateFeedback(int orderId, FeedbackRequest feedbackRequest)
    {
        await _handlerFactory.Get<CreateFeedbackHandler>().HandleAsync(orderId, feedbackRequest);
    }

    public async Task UpdateFeedback(int feedbackId, FeedbackRequest feedbackRequest)
    {
        await _handlerFactory.Get<UpdateFeedbackHandler>().HandleAsync(feedbackId, feedbackRequest);
    }

    public async Task DeleteFeedback(int feedbackId)
    {
        await _handlerFactory.Get<DeleteOrderFeedbackHandler>().HandleAsync(feedbackId);
    }
}