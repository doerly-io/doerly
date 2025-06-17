using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers;

public class UpdateLanguageProficiencyHandler(ProfileDbContext dbContext) : BaseProfileHandler(dbContext)
{
    public async Task<OperationResult> HandleAsync(
        int userId, 
        int proficiencyId, 
        LanguageProficiencySaveDto dto, 
        CancellationToken cancellationToken = default)
    {
        var profileResult = await GetProfileByUserIdAsync(userId, cancellationToken);
        
        if (!profileResult.IsSuccess)
            return profileResult;
        
        var profile = profileResult.Value;
        var proficiency = await DbContext.LanguageProficiencies
            .FirstOrDefaultAsync(lp => lp.ProfileId == profile!.Id && lp.Id == proficiencyId, cancellationToken);

        if (proficiency == null)
            return OperationResult.Failure(Resources.Get("LanguageProficiencyNotFound"));

        if (proficiency.LanguageId != dto.LanguageId)
        {
            var languageResult = await ValidateLanguageAsync(dto.LanguageId, cancellationToken);
            if (!languageResult.IsSuccess)
                return languageResult;

            var duplicateResult = await CheckDuplicateLanguageProficiencyAsync(
                profile!.Id, dto.LanguageId, proficiencyId, cancellationToken);
            if (!duplicateResult.IsSuccess)
                return duplicateResult;
            
            proficiency.LanguageId = dto.LanguageId;
        }

        proficiency.Level = dto.Level;
        await DbContext.SaveChangesAsync(cancellationToken);

        return OperationResult.Success();
    }

}