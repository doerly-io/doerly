using Doerly.Domain.Models;
using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.Domain.Dtos;
using Doerly.Module.Profile.Localization;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers;

public class UpdateProfileHandler(ProfileDbContext dbContext) : BaseProfileHandler(dbContext)
{
    public async Task<HandlerResult> HandleAsync(ProfileSaveDto dto)
    {
        var profile = await DbContext.Profiles
            .FirstOrDefaultAsync(x => x.UserId == dto.UserId);
        
        if (profile == null)
            return HandlerResult.Failure(Resources.Get("ProfileNotFound"));

        CopyToProfileFromDto(profile, dto);
        await DbContext.SaveChangesAsync();
        
        return HandlerResult.Success();
    }
}