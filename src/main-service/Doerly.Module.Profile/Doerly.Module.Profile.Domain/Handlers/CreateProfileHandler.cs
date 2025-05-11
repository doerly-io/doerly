using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.DataAccess.Models;
using Microsoft.EntityFrameworkCore;


namespace Doerly.Module.Profile.Domain.Handlers;

public class CreateProfileHandler(ProfileDbContext dbContext) : BaseProfileHandler(dbContext)
{
    public async Task<HandlerResult> HandleAsync(ProfileSaveDto dto, CancellationToken cancellationToken = default)
    {
        var isProfileExists = DbContext.Profiles
            .AsNoTracking()
            .Any(x => x.UserId == dto.UserId);
        
        if (isProfileExists)
            return HandlerResult.Failure(Resources.Get("ProfileAlreadyExist"));
        
        var profile = new ProfileEntity();
        CopyFromDto(profile, dto);
        await DbContext.Profiles.AddAsync(profile, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
        return HandlerResult.Success();
    }
}
