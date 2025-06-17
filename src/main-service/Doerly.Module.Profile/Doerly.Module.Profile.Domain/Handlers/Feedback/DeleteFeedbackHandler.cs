using Doerly.Module.Profile.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers.Feedback;

public class DeleteOrderFeedbackHandler : BaseProfileHandler
{
    public DeleteOrderFeedbackHandler(ProfileDbContext dbContext) : base(dbContext)
    {
    }

    public async Task HandleAsync(int feedbackId)
    {
        await DbContext.Feedbacks.Where(x => x.Id == feedbackId)
            .ExecuteDeleteAsync();
    }
}