using Doerly.Domain.Models;
using Doerly.FileRepository;
using Doerly.Localization;
using Doerly.Module.Common.DataAccess.Address;
using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers;

public class GetProfileHandler(ProfileDbContext dbContext, AddressDbContext addressDbContext, IFileRepository fileRepository) : BaseProfileHandler(dbContext)
{
    public async Task<OperationResult<ProfileDto>> HandleAsync(int userId, CancellationToken cancellationToken = default)
    {
        var profile = await GetCompleteProfileQuery()
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);

        if (profile == null)
            return OperationResult.Failure<ProfileDto>(Resources.Get("ProfileNotFound"));
        
        var languageProficiencies = profile.LanguageProficiencies
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
            .ToList();
        
        var competences = profile.Competences
            .Select(c => new CompetenceDto
            {
                Id = c.Id,
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName
            })
            .ToList();

        var addressTask = profile.CityId.HasValue 
            ? addressDbContext.Cities
                .AsNoTracking()
                .Where(c => c.Id == profile.CityId.Value)
                .Include(c => c.Region)
                .Select(c => new ProfileAddressDto
                {
                    CityId = c.Id,
                    CityName = c.Name,
                    RegionId = c.Region.Id,
                    RegionName = c.Region.Name
                })
                .FirstOrDefaultAsync(cancellationToken)
            : Task.FromResult<ProfileAddressDto?>(null);
            
        var fileUrls = await GetFileUrlsBatchAsync(new[] { profile }, fileRepository);
        fileUrls.TryGetValue(profile.Id, out var urls);

        var address = await addressTask;

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
            ImageUrl = urls.ImageUrl,
            CvUrl = urls.CvUrl,
            Address = address,
            LanguageProficiencies = languageProficiencies,
            Competences = competences
        };

        return OperationResult.Success(profileDto);
    }
}
