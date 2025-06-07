using Doerly.Domain.Models;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers;

public class GetAllShortProfilesHandler(ProfileDbContext dbContext) : BaseProfileHandler(dbContext)
{
    public async Task<HandlerResult<IEnumerable<ProfileShortInfoDto>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var profiles = await DbContext.Profiles
            .Select(p => new ProfileShortInfoDto
            {
                Id = p.UserId,
                FirstName = p.FirstName,
                LastName = p.LastName
            })
            .ToListAsync(cancellationToken);
        
        return HandlerResult.Success<IEnumerable<ProfileShortInfoDto>>(profiles);
    }
}
