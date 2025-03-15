using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Profile.DataAccess;
using ProfileModel = Doerly.Module.Profile.DataAccess.Models.Profile;
using Doerly.Module.Profile.Domain.Dtos;
using Microsoft.EntityFrameworkCore;


namespace Doerly.Module.Profile.Domain.Handlers;

public class CreateProfileHandler(ProfileDbContext dbContext) : BaseProfileHandler(dbContext)
{
    public async Task<HandlerResult> HandleAsync(ProfileSaveDto dto)
    {
        var isProfileExists = DbContext.Profiles
            .AsNoTracking()
            .Any(x => x.UserId == dto.UserId);
        
        if (isProfileExists)
            return HandlerResult.Failure(Resources.Get("ProfileAlreadyExist"));
        
        var profile = new ProfileModel();
        CopyFromDto(profile, dto);
        DbContext.Profiles.Add(profile);
        await DbContext.SaveChangesAsync();
        
        return HandlerResult.Success();
    }
}