using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain;

public class ProfileService(ProfileDbContext dbContext)
{
    public async Task<(DataAccess.Models.Profile? Profile, HandlerResult Result)> GetProfileByUserIdAsync(
        int userId, 
        CancellationToken cancellationToken = default)
    {
        var profile = await dbContext.Profiles
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);

        if (profile == null)
            return (null, HandlerResult.Failure(Resources.Get("ProfileNotFound")));

        return (profile, HandlerResult.Success());
    }

    public async Task<HandlerResult> ValidateProfileExistsAsync(
        int userId, 
        bool shouldExist = true, 
        CancellationToken cancellationToken = default)
    {
        var exists = await dbContext.Profiles
            .AsNoTracking()
            .AnyAsync(p => p.UserId == userId, cancellationToken);

        if (shouldExist && !exists)
            return HandlerResult.Failure(Resources.Get("ProfileNotFound"));

        if (!shouldExist && exists)
            return HandlerResult.Failure(Resources.Get("ProfileAlreadyExist"));

        return HandlerResult.Success();
    }

    public async Task<HandlerResult> ValidateLanguageAsync(
        int languageId, 
        CancellationToken cancellationToken = default)
    {
        var exists = await dbContext.Languages
            .AnyAsync(l => l.Id == languageId, cancellationToken);

        if (!exists)
            return HandlerResult.Failure(Resources.Get("LanguageNotFound"));

        return HandlerResult.Success();
    }

    public async Task<HandlerResult> CheckDuplicateLanguageProficiencyAsync(
        int profileId, 
        int languageId, 
        int? excludeProficiencyId = null, 
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.LanguageProficiencies
            .Where(lp => lp.ProfileId == profileId && lp.LanguageId == languageId);

        if (excludeProficiencyId.HasValue)
            query = query.Where(lp => lp.Id != excludeProficiencyId.Value);

        var exists = await query.AnyAsync(cancellationToken);

        if (exists)
            return HandlerResult.Failure(Resources.Get("LanguageProficiencyAlreadyExists"));

        return HandlerResult.Success();
    }

    public void MapProfileFromDto(DataAccess.Models.Profile profile, ProfileSaveDto dto)
    {
        profile.FirstName = dto.FirstName;
        profile.LastName = dto.LastName;
        profile.UserId = dto.UserId;
        profile.DateOfBirth = dto.DateOfBirth;
        profile.Sex = dto.Sex;
        profile.Bio = dto.Bio;
        profile.StreetId = dto.StreetId;
        profile.LastModifiedDate = DateTime.UtcNow;
    }

}