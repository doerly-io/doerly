using Doerly.DataTransferObjects;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers.Review;

public class GetProfileReviewsHandler(ProfileDbContext dbContext) : BaseProfileHandler(dbContext)
{
    public async Task<CursorPaginationResponse<ReviewResponseDto>> HandleAsync(int profileId, CursorPaginationRequest paginationRequest)
    {
        var cursor = Cursor.Decode(paginationRequest.Cursor);

        var reviewsQuery = DbContext.Reviews
            .AsNoTracking()
            .Include(r => r.ReviewerProfile)
            .Where(r => r.ProfileId == profileId);

        if (cursor.LastId.HasValue)
            reviewsQuery = reviewsQuery.Where(r => r.Id > cursor.LastId.Value);

        var reviews = await reviewsQuery
            .OrderByDescending(r => r.DateCreated)
            .Take(paginationRequest.PageSize + 1)
            .Select(x => new ReviewResponseDto
            {
                Id = x.Id,
                Comment = x.Comment,
                Rating = x.Rating,
                CreatedAt = x.DateCreated,
                ReviewerProfile = new ReviewerProfileResponseDto
                {
                    ProfileId = x.ReviewerProfile.Id,
                    AvatarUrl = x.ReviewerProfile.ImagePath,
                    FullName = $"{x.ReviewerProfile.FirstName} {x.ReviewerProfile.LastName}"
                }
            })
            .ToListAsync();

        var hasMore = reviews.Count > paginationRequest.PageSize;
        var lastId = hasMore ? reviews.Last().Id : default(int?);

        var newCursor = Cursor.Encode(lastId);
        var response = new CursorPaginationResponse<ReviewResponseDto>
        {
            Items = reviews.Take(paginationRequest.PageSize),
            Cursor = newCursor,
        };

        return response;
    }
}
