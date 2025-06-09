using Doerly.Domain.Models;
using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers;

public class GetAllShortProfilesHandler(ProfileDbContext dbContext) : BaseProfileHandler(dbContext)
{
    public async Task<OperationResult<IEnumerable<ProfileShortInfoDto>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var profiles = await DbContext.Profiles
            .Select(p => new ProfileShortInfoDto
            {
                Id = p.UserId,
                FirstName = p.FirstName,
                LastName = p.LastName
            })
            .ToListAsync(cancellationToken);
        
        return OperationResult.Success<IEnumerable<ProfileShortInfoDto>>(profiles);
    }
}
