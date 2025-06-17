using Doerly.DataTransferObjects;
using Doerly.FileRepository;
using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.DataTransferObjects.Feedback;
using Doerly.Module.Profile.DataTransferObjects.Profile;
using Doerly.Module.Profile.Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers.Feedback;

public class SelectUserFeedbacksHandler : BaseProfileHandler
{
    private readonly IFileRepository _fileRepository;

    public SelectUserFeedbacksHandler(ProfileDbContext dbContext,
        IFileRepository fileRepository)
        : base(dbContext)
    {
        _fileRepository = fileRepository;
    }

    public async Task<CursorPaginationResponse<FeedbackResponse>> HandleAsync(int userId, CursorPaginationRequest request)
    {
        var query = DbContext.Feedbacks
            .AsNoTracking()
            .Where(p => p.RevieweeUserId == userId);

        var cursor = Cursor.Decode(request.Cursor);
        if (cursor.LastId != null)
            query = query.Where(p => p.Id > cursor.LastId);

        var feedbacks = await query
            .OrderByDescending(x => x.LastModifiedDate)
            .Select(x => new FeedbackResponse
            {
                FeedbackId = x.Id,
                Comment = x.Comment,
                Rating = x.Rating,
                CreatedAt = x.DateCreated,
                UpdatedAt = x.LastModifiedDate == x.DateCreated ? null : x.LastModifiedDate,
                UserProfile = new ProfileInfo
                {
                    Id = x.ReviewerProfile.Id,
                    UserId = x.ReviewerUserId,
                    AvatarUrl = x.ReviewerProfile.ImagePath,
                    FirstName = x.ReviewerProfile.FirstName,
                    LastName = x.ReviewerProfile.LastName,
                }
            })
            .Take(request.PageSize + 1)
            .ToListAsync();
        
        await UpdateAvatarUrlsAsync(feedbacks.Select(x => x.UserProfile));

        var hasMode = feedbacks.Count > request.PageSize;
        string? nextCursor = null;
        if (hasMode)
        {
            var lastId = feedbacks.Last().FeedbackId;
            nextCursor = Cursor.Encode(lastId);
            feedbacks.RemoveAt(request.PageSize);
        }

        return new CursorPaginationResponse<FeedbackResponse>
        {
            Cursor = nextCursor,
            Items = feedbacks
        };
    }
    
    private async Task UpdateAvatarUrlsAsync(IEnumerable<ProfileInfo> profiles)
    {
        var tasks = profiles
            .Where(x => !string.IsNullOrEmpty(x.AvatarUrl))
            .Select(async p =>
            {
                p.AvatarUrl = await _fileRepository.GetSasUrlAsync(AzureStorageConstants.ImagesContainerName, p.AvatarUrl);
            })
            .ToList();
        
        await Task.WhenAll(tasks);
    }
}
