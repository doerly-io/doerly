using Doerly.Domain.Models;
using Doerly.FileRepository;
using Doerly.Localization;
using Doerly.Module.Common.DataAccess.Address;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers;

public class GetProfilesHandler(ProfileDbContext dbContext, AddressDbContext addressDbContext, IFileRepository fileRepository) : BaseProfileHandler(dbContext)
{
    public async Task<HandlerResult<IEnumerable<ProfileDto>>> HandleAsync(int[] userIds, CancellationToken cancellationToken = default)
    {
        var distinctUserIds = userIds.Distinct().ToArray();

        var profiles = await GetCompleteProfileQuery()
            .Where(p => distinctUserIds.Contains(p.UserId))
            .ToListAsync(cancellationToken);

        if (profiles.Count != distinctUserIds.Length)
        {
            return HandlerResult.Failure<IEnumerable<ProfileDto>>(Resources.Get("ProfileNotFound"));
        }

        var profileDtos = await MapCompleteProfilesToDtosAsync(profiles, addressDbContext, fileRepository, cancellationToken);

        return HandlerResult.Success<IEnumerable<ProfileDto>>(profileDtos);
    }
}
