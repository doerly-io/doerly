using Doerly.Domain.Models;
using Doerly.Module.Order.DataTransferObjects.Requests;
using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.DataTransferObjects.Feedback;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers.Feedback;

public class UpdateFeedbackHandler : BaseProfileHandler
{
    public UpdateFeedbackHandler(ProfileDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<OperationResult> HandleAsync(int feedbackId, FeedbackRequest request)
    {
        await DbContext.Feedbacks.Where(x => x.Id == feedbackId)
            .ExecuteUpdateAsync(x => x
                .SetProperty(y => y.Comment, request.Comment)
                .SetProperty(b => b.LastModifiedDate, DateTime.UtcNow)
                .SetProperty(y => y.Rating, request.Rating));
        
        return OperationResult.Success();
    }
}
