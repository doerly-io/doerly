using Doerly.Domain.Handlers;
using Doerly.Domain.Models;
using Doerly.FileRepository;
using Doerly.Localization;
using Doerly.Module.Authorization.Contracts.Responses;
using Doerly.Module.Common.DataAccess.Address;
using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.DataAccess.Entities;
using Doerly.Module.Profile.Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers;

public class BaseProfileHandler(ProfileDbContext dbContext) : BaseHandler<ProfileDbContext>(dbContext)
{
    #region Profile Methods
    
    protected IQueryable<DataAccess.Entities.Profile> GetCompleteProfileQuery()
    {
        return DbContext.Profiles
            .Include(p => p.LanguageProficiencies!)
                .ThenInclude(lp => lp.Language)
            .Include(p => p.Competences!)
            .AsNoTracking();
    }
    
    protected async Task<HandlerResult<DataAccess.Entities.Profile?>> GetProfileByUserIdAsync(
        int userId, 
        CancellationToken cancellationToken = default)
    {
        var profile = await DbContext.Profiles
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);

        if (profile == null)
            return HandlerResult.Failure<DataAccess.Entities.Profile?>(Resources.Get("ProfileNotFound"));

        return HandlerResult.Success(profile);
    }

    protected async Task<HandlerResult> ValidateProfileExistsAsync(
        int userId, 
        bool shouldExist = true, 
        CancellationToken cancellationToken = default)
    {
        var exists = await DbContext.Profiles
            .AsNoTracking()
            .AnyAsync(p => p.UserId == userId, cancellationToken);

        if (shouldExist && !exists)
            return HandlerResult.Failure(Resources.Get("ProfileNotFound"));

        if (!shouldExist && exists)
            return HandlerResult.Failure(Resources.Get("ProfileAlreadyExist"));

        return HandlerResult.Success();
    }
    
    protected void MapProfileFromDto(DataAccess.Entities.Profile profile, ProfileSaveDto dto)
    {
        profile.FirstName = dto.FirstName;
        profile.LastName = dto.LastName;
        profile.UserId = dto.UserId;
        profile.DateOfBirth = dto.DateOfBirth;
        profile.Sex = dto.Sex;
        profile.Bio = dto.Bio;
        profile.CityId = dto.CityId;
        profile.LastModifiedDate = DateTime.UtcNow;
    }
    
    protected async Task<List<ProfileDto>> MapCompleteProfilesToDtosAsync(
        IEnumerable<DataAccess.Models.Profile> profiles,
        AddressDbContext addressDbContext,
        IFileRepository fileRepository,
        CancellationToken cancellationToken = default,
        IEnumerable<UserItemResponse>? profilesUsers = null)
    {
        var profileList = profiles.ToList();
        if (!profileList.Any())
            return new List<ProfileDto>();

        var cityIds = profileList.Select(p => p.CityId ?? 0).Where(id => id > 0).Distinct().ToArray();

        var addressesTask = GetAddressesBatchAsync(cityIds, addressDbContext, cancellationToken);
        var fileUrlsTask = GetFileUrlsBatchAsync(profileList, fileRepository);

        await Task.WhenAll(addressesTask, fileUrlsTask);

        var addressMap = await addressesTask;
        var fileUrlsMap = await fileUrlsTask;

        var profileDtos = new List<ProfileDto>();

        foreach (var profile in profileList)
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
            
            var profileUser = profilesUsers?.FirstOrDefault(u => u.UserId == profile.UserId);
            var userInfo = profileUser != null ? new UserInfo
            {
                UserId = profileUser.UserId,
                Email = profileUser.Email,
                IsEnabled = profileUser.IsEnabled,
                IsEmailVerified = profileUser.IsEmailVerified,
                RoleName = profileUser.RoleName,
                RoleId = profileUser.RoleId
            } : null;
            
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
                Competences = competences,
                UserInfo = userInfo
            });
        }

        return profileDtos;
    }
    
    #endregion
    
    #region Language Methods
    
    protected async Task<HandlerResult> ValidateLanguageAsync(
        int languageId, 
        CancellationToken cancellationToken = default)
    {
        var exists = await DbContext.Languages
            .AnyAsync(l => l.Id == languageId, cancellationToken);

        if (!exists)
            return HandlerResult.Failure(Resources.Get("LanguageNotFound"));

        return HandlerResult.Success();
    }
    
    protected async Task<HandlerResult<Language?>> GetLanguageByIdAsync(
        int languageId, 
        CancellationToken cancellationToken = default)
    {
        var language = await DbContext.Languages
            .FirstOrDefaultAsync(l => l.Id == languageId, cancellationToken);

        if (language == null)
            return HandlerResult.Failure<Language?>(Resources.Get("LanguageNotFound"));

        return HandlerResult.Success(language);
    }
    
    protected async Task<HandlerResult> ValidateLanguageIsUniqueAsync(
        string name, 
        string code, 
        int? excludeId = null, 
        CancellationToken cancellationToken = default)
    {
        var query = DbContext.Languages.AsQueryable();
        
        if (excludeId.HasValue)
            query = query.Where(l => l.Id != excludeId.Value);
            
        var exists = await query.AnyAsync(l => l.Name == name || l.Code == code, cancellationToken);

        if (exists)
            return HandlerResult.Failure(Resources.Get("LanguageAlreadyExists"));

        return HandlerResult.Success();
    }
    
    #endregion
    
    #region Language Proficiency Methods
    
    protected async Task<HandlerResult<LanguageProficiency?>> GetLanguageProficiencyAsync(
        int profileId, 
        int proficiencyId, 
        CancellationToken cancellationToken = default)
    {
        var proficiency = await DbContext.LanguageProficiencies
            .FirstOrDefaultAsync(lp => lp.ProfileId == profileId && lp.Id == proficiencyId, cancellationToken);

        if (proficiency == null)
            return HandlerResult.Failure<LanguageProficiency?>(Resources.Get("LanguageProficiencyNotFound"));

        return HandlerResult.Success(proficiency);
    }
    
    protected async Task<HandlerResult> CheckDuplicateLanguageProficiencyAsync(
        int profileId, 
        int languageId, 
        int? excludeProficiencyId = null, 
        CancellationToken cancellationToken = default)
    {
        var query = DbContext.LanguageProficiencies
            .Where(lp => lp.ProfileId == profileId && lp.LanguageId == languageId);

        if (excludeProficiencyId.HasValue)
            query = query.Where(lp => lp.Id != excludeProficiencyId.Value);

        var exists = await query.AnyAsync(cancellationToken);

        if (exists)
            return HandlerResult.Failure(Resources.Get("LanguageProficiencyAlreadyExists"));

        return HandlerResult.Success();
    }
    
    #endregion

    #region Batch Data Retrieval Methods (External Dependencies)

    protected async Task<Dictionary<int, ProfileAddressDto>> GetAddressesBatchAsync(
        IEnumerable<int> cityIds,
        AddressDbContext addressDbContext,
        CancellationToken cancellationToken = default)
    {
        var distinctCityIds = cityIds.Where(id => id > 0).Distinct().ToArray();
        if (!distinctCityIds.Any())
            return new Dictionary<int, ProfileAddressDto>();

        return await addressDbContext.Cities
            .AsNoTracking()
            .Where(c => distinctCityIds.Contains(c.Id))
            .Include(c => c.Region) 
            .Select(c => new ProfileAddressDto
            {
                CityId = c.Id,
                CityName = c.Name,
                RegionId = c.Region.Id,
                RegionName = c.Region.Name
            })
            .ToDictionaryAsync(a => a.CityId, a => a, cancellationToken);
    }

    protected async Task<Dictionary<int, (string? ImageUrl, string? CvUrl)>> GetFileUrlsBatchAsync(
        IEnumerable<DataAccess.Entities.Profile> profiles,
        IFileRepository fileRepository)
    {
        var urlTasksByProfileId = new Dictionary<int, (Task<string?> ImageTask, Task<string?> CvTask)>();
        var allUrlTasks = new List<Task>();

        foreach (var profile in profiles)
        {
            Task<string?> imageTask = Task.FromResult<string?>(null);
            if (!string.IsNullOrEmpty(profile.ImagePath))
            {
                imageTask = fileRepository.GetSasUrlAsync(
                    AzureStorageConstants.ImagesContainerName,
                    profile.ImagePath);
                allUrlTasks.Add(imageTask);
            }

            Task<string?> cvTask = Task.FromResult<string?>(null);
            if (!string.IsNullOrEmpty(profile.CvPath))
            {
                cvTask = fileRepository.GetSasUrlAsync(
                    AzureStorageConstants.DocumentsContainerName,
                    profile.CvPath);
                allUrlTasks.Add(cvTask);
            }
            urlTasksByProfileId[profile.Id] = (imageTask, cvTask);
        }

        if (allUrlTasks.Any())
        {
            await Task.WhenAll(allUrlTasks);
        }

        var results = new Dictionary<int, (string? ImageUrl, string? CvUrl)>();
        foreach (var entry in urlTasksByProfileId)
        {
            results[entry.Key] = (await entry.Value.ImageTask, await entry.Value.CvTask);
        }
        return results;
    }

    #endregion
}
