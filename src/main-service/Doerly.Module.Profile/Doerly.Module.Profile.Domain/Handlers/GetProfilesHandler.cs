using Doerly.Domain.Models;
using Doerly.FileRepository;
using Doerly.Localization;
using Doerly.Module.Common.DataAccess.Address;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers;

public class GetProfilesHandler(ProfileDbContext dbContext, AddressDbContext addressDbContext, IFileRepository fileRepository) : BaseProfileHandler(dbContext)
{
    public async Task<HandlerResult<IEnumerable<ProfileDto>>> HandleAsync(int[] userIds, CancellationToken cancellationToken = default)
    {
        var distinctUserIds = userIds.Distinct().ToArray();

        var profiles = await GetCompleteProfileQuery()
            .Where(p => distinctUserIds.Contains(p.UserId))
            .ToListAsync(cancellationToken);

        if (profiles.Count != distinctUserIds.Length)
        {
            return HandlerResult.Failure<IEnumerable<ProfileDto>>(Resources.Get("ProfileNotFound"));
        }

        var cityIds = profiles.Select(p => p.CityId ?? 0).Where(id => id > 0).Distinct().ToArray();

        var addressesTask = GetAddressesBatchAsync(cityIds, addressDbContext, cancellationToken);
        var fileUrlsTask = GetFileUrlsBatchAsync(profiles, fileRepository);

        await Task.WhenAll(addressesTask, fileUrlsTask);

        var addressMap = await addressesTask;
        var fileUrlsMap = await fileUrlsTask;

        var profileDtos = new List<ProfileDto>();

        foreach (var profile in profiles)
        {
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

            addressMap.TryGetValue(profile.CityId ?? 0, out var address);
            fileUrlsMap.TryGetValue(profile.Id, out var urls);

            profileDtos.Add(new ProfileDto
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
            });
        }

        return HandlerResult.Success<IEnumerable<ProfileDto>>(profileDtos);
    }
}