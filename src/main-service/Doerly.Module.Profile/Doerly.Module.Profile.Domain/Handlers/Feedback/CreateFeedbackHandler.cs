using Doerly.Domain;
using Doerly.Domain.Models;
using Doerly.Module.Order.DataTransferObjects.Requests;
using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.DataAccess.Models;
using Doerly.Module.Profile.DataTransferObjects.Feedback;

namespace Doerly.Module.Profile.Domain.Handlers.Feedback;

public class CreateFeedbackHandler : BaseProfileHandler
{
    private readonly IDoerlyRequestContext _doerlyRequestContext;

    public CreateFeedbackHandler(ProfileDbContext dbContext, IDoerlyRequestContext doerlyRequestContext) : base(dbContext)
    {
        _doerlyRequestContext = doerlyRequestContext;
    }

    public async Task<OperationResult> HandleAsync(int orderId, FeedbackRequest request)
    {
        var feedback = new FeedbackEntity
        {
            Comment = request.Comment,
            Rating = request.Rating,
            OrderId = orderId,
            CategoryId = request.CategoryId,
            ReviewerUserId = _doerlyRequestContext.UserId ?? -1,
            RevieweeUserId = request.ExecutorId
        };
        
        DbContext.Feedbacks.Add(feedback);
        await DbContext.SaveChangesAsync();
        
        return OperationResult.Success();
    }
}
