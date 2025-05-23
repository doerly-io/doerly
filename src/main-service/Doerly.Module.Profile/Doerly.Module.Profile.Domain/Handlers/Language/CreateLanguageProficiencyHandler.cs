using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers;

public class CreateLanguageProficiencyHandler(ProfileDbContext dbContext) : BaseProfileHandler(dbContext) 
{
    public async Task<HandlerResult> HandleAsync(int userId, LanguageProficiencySaveDto dto, CancellationToken cancellationToken = default)
    {
        var (profile, profileResult) = await GetProfileByUserIdAsync(userId, cancellationToken);
        
        if (!profileResult.IsSuccess)
            return profileResult;
        
        var languageResult = await ValidateLanguageAsync(dto.LanguageId, cancellationToken);
        if (!languageResult.IsSuccess)
            return languageResult;
        
        var duplicateResult = await CheckDuplicateLanguageProficiencyAsync(
            profile!.Id, dto.LanguageId, cancellationToken: cancellationToken);
        if (!duplicateResult.IsSuccess)
            return duplicateResult;
        
        var newLanguageProficiency = new LanguageProficiency
        {
            ProfileId = profile.Id,
            LanguageId = dto.LanguageId,
            Level = dto.Level
        };
        
        DbContext.LanguageProficiencies.Add(newLanguageProficiency);
        await DbContext.SaveChangesAsync(cancellationToken);
        return HandlerResult.Success();
    }
    
}