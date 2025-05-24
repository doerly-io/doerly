using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers;

public class CreateCompetenceHandler(ProfileDbContext dbContext) : BaseProfileHandler(dbContext)
{
    public async Task<HandlerResult> HandleAsync(int userId, CompetenceSaveDto dto, CancellationToken cancellationToken = default)
    {
        var (profile, profileResult) = await GetProfileByUserIdAsync(userId, cancellationToken);
        
        if (!profileResult.IsSuccess)
            return profileResult;
        
        var isCompetenceExists = await DbContext.Competences
            .AnyAsync(c => c.ProfileId == profile.Id && c.CategoryId == dto.CategoryId, cancellationToken);
        
        if (isCompetenceExists)
            return HandlerResult.Failure(Resources.Get("CompetenceAlreadyExists"));
        
        var newCompetence = new DataAccess.Models.Competence
        {
            ProfileId = profile.Id,
            CategoryId = dto.CategoryId,
            CategoryName = dto.CategoryName
        };
        
        DbContext.Competences.Add(newCompetence);
        await DbContext.SaveChangesAsync(cancellationToken);
        return HandlerResult.Success();
    }
}