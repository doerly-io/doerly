using Doerly.Domain.Models;
using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.Contracts.Dtos;

namespace Doerly.Module.Profile.Domain.Handlers;

public class UpdateProfileHandler(ProfileDbContext dbContext) : BaseProfileHandler(dbContext)
{
    public async Task<HandlerResult> HandleAsync(ProfileSaveDto dto, CancellationToken cancellationToken = default)
    {
        var (profile, result) = await GetProfileByUserIdAsync(dto.UserId, cancellationToken);
        
        if (!result.IsSuccess)
            return result;
        
        MapProfileFromDto(profile!, dto);
        await DbContext.SaveChangesAsync(cancellationToken);
        
        return HandlerResult.Success();
    }
}
