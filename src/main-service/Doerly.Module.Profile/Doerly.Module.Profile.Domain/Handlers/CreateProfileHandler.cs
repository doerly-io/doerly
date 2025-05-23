using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.DataAccess;
using Microsoft.EntityFrameworkCore;


namespace Doerly.Module.Profile.Domain.Handlers;

public class CreateProfileHandler(ProfileDbContext dbContext) : BaseProfileHandler(dbContext)
{
    public async Task<HandlerResult> HandleAsync(ProfileSaveDto dto, CancellationToken cancellationToken = default)
    {
        var validationResult = await ValidateProfileExistsAsync(
            dto.UserId, shouldExist: false, cancellationToken);
        
        if (!validationResult.IsSuccess)
            return validationResult;
        
        var profile = new DataAccess.Entities.Profile();
        MapProfileFromDto(profile, dto);

        DbContext.Profiles.Add(profile);
        await DbContext.SaveChangesAsync(cancellationToken);
        
        return HandlerResult.Success();
    }
}
