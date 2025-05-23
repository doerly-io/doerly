using Doerly.Domain.Models;
using Doerly.FileRepository;
using Doerly.Localization;
using Doerly.Module.Common.DataAccess.Address;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers;

public class GetProfileHandler(ProfileDbContext dbContext, AddressDbContext addressDbContext, IFileRepository fileRepository) : BaseProfileHandler(dbContext)
{
    public async Task<HandlerResult<ProfileDto>> HandleAsync(int userId, CancellationToken cancellationToken = default)
    {
        var profile = await DbContext.Profiles
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);

        if (profile == null)
            return HandlerResult.Failure<ProfileDto>(Resources.Get("ProfileNotFound"));
        
        var languageProficiencies = await DbContext.LanguageProficiencies
            .AsNoTracking()
            .Where(lp => lp.ProfileId == profile.Id)
            .Select(lp => new LanguageProficiencyDto
            {
                Id = lp.Id,
                Language = new LanguageDto 
                {
                    Id = lp.Language.Id,
                    Name = lp.Language.Name,
                    Code = lp.Language.Code
                },
                Level = lp.Level
            })
            .ToListAsync(cancellationToken);

        var urlTasks = new List<Task>(2);
        string imageUrl = null;
        string cvUrl = null;
    
        if (!string.IsNullOrEmpty(profile.ImagePath))
        {
            urlTasks.Add(Task.Run(async () => 
            {
                imageUrl = await fileRepository.GetSasUrlAsync(
                    AzureStorageConstants.ImagesContainerName, 
                    profile.ImagePath);
            }));
        }
    
        if (!string.IsNullOrEmpty(profile.CvPath))
        {
            urlTasks.Add(Task.Run(async () => 
            {
                cvUrl = await fileRepository.GetSasUrlAsync(
                    AzureStorageConstants.DocumentsContainerName, 
                    profile.CvPath);
            }));
        }
    
        if (urlTasks.Count > 0)
        {
            await Task.WhenAll(urlTasks);
        }
        
        var address = await addressDbContext.Cities
            .AsNoTracking()
            .Where(c => c.Id == profile.CityId)
            .Select(c => new ProfileAddressDto
            {
                CityId = c.Id,
                CityName = c.Name,
                RegionId = c.Region.Id,
                RegionName = c.Region.Name
            })
            .FirstOrDefaultAsync(cancellationToken);

        var profileDto = new ProfileDto
        {
            Id = profile.Id,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            DateOfBirth = profile.DateOfBirth,
            Sex = profile.Sex,
            Bio = profile.Bio,
            DateCreated = profile.DateCreated,
            LastModifiedDate = profile.LastModifiedDate,
            ImageUrl = imageUrl,
            CvUrl = cvUrl,
            Address = address,
            LanguageProficiencies = languageProficiencies
        };

        return HandlerResult.Success(profileDto);
    }
}
