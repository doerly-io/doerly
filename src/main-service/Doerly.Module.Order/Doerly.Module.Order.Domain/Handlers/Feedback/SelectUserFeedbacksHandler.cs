using Doerly.DataTransferObjects;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.DataTransferObjects;
using Doerly.Module.Order.DataTransferObjects.Responses;
using Doerly.Module.Order.Enums;
using Doerly.Proxy.Profile;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Order.Domain.Handlers;

public class SelectUserFeedbacksHandler : BaseOrderHandler
{
    private readonly IProfileModuleProxy _profileModuleProxy;

    public SelectUserFeedbacksHandler(OrderDbContext dbContext, IProfileModuleProxy profileModuleProxy)
        : base(dbContext)
    {
        _profileModuleProxy = profileModuleProxy;
    }

    public async Task<CursorPaginationResponse<OrderFeedbackResponse>> HandleAsync(int userId, CursorPaginationRequest request)
    {
        var query = DbContext.OrderFeedbacks
            .AsNoTracking()
            .Where(p => p.Order.ExecutorId == userId && p.Order.Status == EOrderStatus.Completed);

        var cursor = Cursor.Decode(request.Cursor);
        if (cursor.LastId != null)
            query = query.Where(p => p.Id > cursor.LastId);

        var feedbacks = await query
            .OrderByDescending(x => x.LastModifiedDate)
            .Select(x => new OrderFeedbackResponse
            {
                FeedbackId = x.Id,
                Comment = x.Comment,
                Rating = x.Rating,
                CreatedAt = x.DateCreated,
                UpdatedAt = x.LastModifiedDate == x.DateCreated ? null : x.LastModifiedDate,
                UserProfile = new ProfileInfo
                {
                    UserId = x.Order.CustomerId
                }
            })
            .Take(request.PageSize + 1)
            .ToListAsync();

        var profiles = await _profileModuleProxy.GetProfilesShortInfoWithAvatarAsync(
            feedbacks.Select(x => x.UserProfile.UserId));

        foreach (var feedback in feedbacks)
        {
            var profile = profiles.FirstOrDefault(p => p.UserId == feedback.UserProfile.UserId);
            if (profile == null) continue;

            feedback.UserProfile.FirstName = profile.FirstName;
            feedback.UserProfile.LastName = profile.LastName;
            feedback.UserProfile.AvatarUrl = profile.AvatarUrl;
            feedback.UserProfile.Id = profile.Id;
        }

        var hasMode = feedbacks.Count > request.PageSize;
        string? nextCursor = null;
        if (hasMode)
        {
            var lastId = feedbacks.Last().FeedbackId;
            nextCursor = Cursor.Encode(lastId);
            feedbacks.RemoveAt(request.PageSize);
        }

        return new CursorPaginationResponse<OrderFeedbackResponse>
        {
            Cursor = nextCursor,
            Items = feedbacks
        };
    }
}
