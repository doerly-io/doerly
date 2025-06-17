using Doerly.FileRepository;
using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.DataTransferObjects.Feedback;
using Doerly.Module.Profile.DataTransferObjects.Profile;
using Doerly.Module.Profile.Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers.Feedback;

public class FeedbackByOrderIdHandler(ProfileDbContext dbContext, IFileRepository fileRepository)
    : BaseProfileHandler(dbContext)
{
    public async Task<FeedbackResponse> HandleAsync(int orderId)
    {
        var feedback = await DbContext.Feedbacks.Where(x => x.OrderId == orderId)
            .Select(x => new FeedbackResponse
            {
                Rating = x.Rating,
                Comment = x.Comment,
                CreatedAt = x.DateCreated,
                UpdatedAt = x.LastModifiedDate == x.DateCreated ? null : x.LastModifiedDate,
                FeedbackId = x.Id,
                UserProfile = new ProfileInfo
                {
                    Id = x.ReviewerProfile.Id,
                    UserId = x.ReviewerUserId,
                    AvatarUrl = x.ReviewerProfile.ImagePath,
                    FirstName = x.ReviewerProfile.FirstName,
                    LastName = x.ReviewerProfile.LastName,
                }
            }).FirstOrDefaultAsync();

        if (feedback?.UserProfile.AvatarUrl != null)
        {
            feedback.UserProfile.AvatarUrl = await fileRepository.GetSasUrlAsync(
                AzureStorageConstants.ImagesContainerName,
                feedback.UserProfile.AvatarUrl);
        }

        return feedback;
    }
}