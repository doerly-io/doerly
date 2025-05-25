using Doerly.Domain.Models;
using Doerly.Module.Profile.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers.Review;

public class DeleteProfileReviewHandler(ProfileDbContext dbContext) : BaseProfileHandler(dbContext)
{
    public async Task<HandlerResult> HandleAsync(int userId, int id)
    {
        await DbContext.Reviews
            .Where(x => x.Id == id && x.ReviewerUserId == userId)
            .ExecuteDeleteAsync();
        
        return HandlerResult.Success();
    }
    
}
