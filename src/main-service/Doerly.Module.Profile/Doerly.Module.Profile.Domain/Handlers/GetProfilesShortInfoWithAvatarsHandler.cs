using Doerly.FileRepository;
using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.DataTransferObjects;
using Doerly.Module.Profile.Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers;

public class GetProfilesShortInfoWithAvatars(ProfileDbContext dbContext, IFileRepository fileRepository) : BaseProfileHandler(dbContext)
{
    public async Task<IEnumerable<ProfileShortInfoWithAvatarDto>> HandleAsync(IEnumerable<int> userIds)
    {
        var profiles = await DbContext.Profiles
            .Where(p => userIds.Contains(p.UserId))
            .Select(p => new ProfileShortInfoWithAvatarDto
            {
                Id = p.Id,
                UserId = p.UserId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                AvatarUrl = p.ImagePath
            })
            .ToListAsync();

        await UpdateAvatarUrlsAsync(profiles);
        return profiles;
    }

    private async Task UpdateAvatarUrlsAsync(IEnumerable<ProfileShortInfoWithAvatarDto> profiles)
    {
        var tasks = profiles
            .Where(x => !string.IsNullOrEmpty(x.AvatarUrl))
            .Select(async p =>
            {
                p.AvatarUrl = await fileRepository.GetSasUrlAsync(AzureStorageConstants.ImagesContainerName, p.AvatarUrl);
            })
            .ToList();
        
        await Task.WhenAll(tasks);
    }
}
