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
    public async Task<HandlerResult<ICollection<ProfileDto>>> HandleAsync(int[] userIds, CancellationToken cancellationToken = default)
    {
        var distinctUserIds = userIds.Distinct().ToArray();

        var profiles = await DbContext.Profiles
            .AsNoTracking()
            .Where(p => distinctUserIds.Contains(p.UserId))
            .ToListAsync(cancellationToken);

        if (profiles.Count != distinctUserIds.Length)
        {
            return HandlerResult.Failure<ICollection<ProfileDto>>(Resources.Get("ProfileNotFound"));
        }

        var profileIds = profiles.Select(p => p.Id).ToArray();
        var cityIds = profiles.Select(p => p.CityId ?? 0).Where(id => id > 0).Distinct().ToArray();

        var languageProficienciesTask = GetLanguageProficienciesBatchAsync(profileIds, cancellationToken);
        var competencesTask = GetCompetencesBatchAsync(profileIds, cancellationToken);
        var addressesTask = GetAddressesBatchAsync(cityIds, addressDbContext, cancellationToken);
        var fileUrlsTask = GetFileUrlsBatchAsync(profiles, fileRepository);

        await Task.WhenAll(languageProficienciesTask, competencesTask, addressesTask, fileUrlsTask);

        var languageProficienciesMap = await languageProficienciesTask;
        var competencesMap = await competencesTask;
        var addressMap = await addressesTask;
        var fileUrlsMap = await fileUrlsTask;

        var profileDtos = new List<ProfileDto>();

        foreach (var profile in profiles)
        {
            languageProficienciesMap.TryGetValue(profile.Id, out var languageProficiencies);
            competencesMap.TryGetValue(profile.Id, out var competences);
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
                LanguageProficiencies = languageProficiencies ?? new List<LanguageProficiencyDto>(),
                Competences = competences ?? new List<CompetenceDto>()
            });
        }

        return HandlerResult.Success<ICollection<ProfileDto>>(profileDtos);
    }
}