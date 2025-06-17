using Doerly.Domain.Models;
using Doerly.Module.Order.DataTransferObjects.Requests;
using Doerly.Module.Profile.DataTransferObjects;
using Doerly.Module.Profile.DataTransferObjects.Feedback;
using Doerly.Proxy.BaseProxy;

namespace Doerly.Proxy.Profile;

public interface IProfileModuleProxy : IModuleProxy
{
    Task<OperationResult<ProfileDto>> GetProfileAsync(int userId);
    Task<OperationResult<IEnumerable<ProfileDto>>> GetProfilesAsync(int[] userIds);
    Task<IEnumerable<ProfileShortInfoWithAvatarDto>> GetProfilesShortInfoWithAvatarAsync(IEnumerable<int> userIds);
    
    Task<FeedbackResponse> GetFeedback(int orderId);
    
    Task CreateFeedback(int orderId, FeedbackRequest feedbackRequest);
    
    Task UpdateFeedback(int feedbackId, FeedbackRequest feedbackRequest);
    
    Task DeleteFeedback(int feedbackId);
}
