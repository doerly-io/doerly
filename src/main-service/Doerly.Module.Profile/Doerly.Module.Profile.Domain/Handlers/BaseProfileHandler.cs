using Doerly.Domain.Handlers;
using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers;

public class BaseProfileHandler(ProfileDbContext dbContext) : BaseHandler<ProfileDbContext>(dbContext)
{
    #region Profile Methods
    
    protected async Task<(DataAccess.Models.Profile? Profile, HandlerResult Result)> GetProfileByUserIdAsync(
        int userId, 
        CancellationToken cancellationToken = default)
    {
        var profile = await DbContext.Profiles
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);

        if (profile == null)
            return (null, HandlerResult.Failure(Resources.Get("ProfileNotFound")));

        return (profile, HandlerResult.Success());
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
    
    protected void MapProfileFromDto(DataAccess.Models.Profile profile, ProfileSaveDto dto)
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
    
    protected async Task<(Language? Language, HandlerResult Result)> GetLanguageByIdAsync(
        int languageId, 
        CancellationToken cancellationToken = default)
    {
        var language = await DbContext.Languages
            .FirstOrDefaultAsync(l => l.Id == languageId, cancellationToken);

        if (language == null)
            return (null, HandlerResult.Failure(Resources.Get("LanguageNotFound")));

        return (language, HandlerResult.Success());
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
    
    protected async Task<(LanguageProficiency? Proficiency, HandlerResult Result)> GetLanguageProficiencyAsync(
        int profileId, 
        int proficiencyId, 
        CancellationToken cancellationToken = default)
    {
        var proficiency = await DbContext.LanguageProficiencies
            .FirstOrDefaultAsync(lp => lp.ProfileId == profileId && lp.Id == proficiencyId, cancellationToken);

        if (proficiency == null)
            return (null, HandlerResult.Failure(Resources.Get("LanguageProficiencyNotFound")));

        return (proficiency, HandlerResult.Success());
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
}
