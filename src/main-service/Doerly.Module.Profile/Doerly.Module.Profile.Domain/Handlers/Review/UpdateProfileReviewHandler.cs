using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers.Review;

public class UpdateProfileReviewHandler(ProfileDbContext dbContext) : BaseProfileHandler(dbContext)
{
    public async Task<HandlerResult> HandleAsync(int userId, int reviewId, UpdateReviewDto dto)
    {
        var foundReviewId = await DbContext.Reviews
            .Where(x => x.Id == reviewId && x.ReviewerUserId == userId)
            .FirstOrDefaultAsync(x => x.Id == reviewId && x.ReviewerUserId == userId);

        if (foundReviewId == null)
            return HandlerResult.Failure(Resources.Get("ReviewNotFoundOrNotOwned"));

        await DbContext.Reviews
            .Where(x => x.Id == reviewId && x.ReviewerUserId == userId)
            .ExecuteUpdateAsync(calls => calls
                .SetProperty(review => review.Comment, dto.Comment.Trim())
                .SetProperty(review => review.Rating, dto.Rating));

        return HandlerResult.Success();
    }
}
