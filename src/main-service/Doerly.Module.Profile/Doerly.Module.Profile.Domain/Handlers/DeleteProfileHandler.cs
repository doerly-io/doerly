using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Profile.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers;

public class DeleteProfileHandler(ProfileDbContext dbContext) : BaseProfileHandler(dbContext)
{
    public async Task<HandlerResult> HandleAsync(int userId)
    {
        var profile = await DbContext.Profiles
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (profile == null)
            return HandlerResult.Failure(Resources.Get("ProfileNotFound"));

        DbContext.Profiles.Remove(profile);
        await DbContext.SaveChangesAsync();
        
        return HandlerResult.Success();
    }
}