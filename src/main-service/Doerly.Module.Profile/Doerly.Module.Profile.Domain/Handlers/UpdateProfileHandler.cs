using Doerly.Domain.Models;
using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.DataTransferObjects;

namespace Doerly.Module.Profile.Domain.Handlers;

public class UpdateProfileHandler(ProfileDbContext dbContext) : BaseProfileHandler(dbContext)
{
    public async Task<OperationResult> HandleAsync(ProfileSaveDto dto, CancellationToken cancellationToken = default)
    {
        var result = await GetProfileByUserIdAsync(dto.UserId, cancellationToken);
        
        if (!result.IsSuccess)
            return result;
        
        var profile = result.Value;
        MapProfileFromDto(profile!, dto);
        await DbContext.SaveChangesAsync(cancellationToken);
        
        return OperationResult.Success();
    }
}
