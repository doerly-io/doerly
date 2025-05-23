using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.DataAccess;

namespace Doerly.Module.Profile.Domain.Handlers.Review;

public class AddProfileReviewHandler(ProfileDbContext dbContext) : BaseProfileHandler(dbContext)
{
    public async Task HandleAsync(AddReviewDto dto)
    {
        var review = new DataAccess.Entities.Review
        {
            Rating = dto.Rating,
            Comment = dto.Comment,
            ReviewerId = dto.ReviewerId,
            RevieweeId = dto.RevieweeId
        };

        await DbContext.Reviews.AddAsync(review);
        await DbContext.SaveChangesAsync();
    }
}
