using System.Linq.Expressions;
using Doerly.DataTransferObjects.Pagination;
using Doerly.Domain.Models;
using Doerly.Extensions;
using Doerly.FileRepository;
using Doerly.Module.Common.DataAccess.Address;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.DataAccess;

namespace Doerly.Module.Profile.Domain.Handlers;

public class SearchProfilesHandler(ProfileDbContext dbContext, AddressDbContext addressDbContext, IFileRepository fileRepository) : BaseProfileHandler(dbContext)
{
    public async Task<HandlerResult<PageDto<ProfileDto>>> HandleAsync(ProfileQueryDto queryDto, CancellationToken cancellationToken = default)
    {
        var predicates = new List<Expression<Func<DataAccess.Models.Profile, bool>>>();
        
        if (!string.IsNullOrWhiteSpace(queryDto.Name))
        {
            var searchTerm = queryDto.Name.Trim().ToLower();
            predicates.Add(p => 
                (p.FirstName + " " + p.LastName).ToLower().Contains(searchTerm) ||
                p.FirstName.ToLower().Contains(searchTerm) ||
                p.LastName.ToLower().Contains(searchTerm));
        }

        var (profiles, totalCount) = await GetCompleteProfileQuery()
            .OrderBy(p => p.FirstName)
            .ThenBy(p => p.LastName)
            .GetEntitiesWithPaginationAsync(queryDto, predicates, cancellationToken: cancellationToken);

        var profileDtos = await MapCompleteProfilesToDtosAsync(profiles, addressDbContext, fileRepository, cancellationToken);

        var pageSize = queryDto.Size > 0 ? queryDto.Size : 10;
        var pagesCount = (int)Math.Ceiling((double)totalCount / pageSize);

        var result = new PageDto<ProfileDto>
        {
            PageSize = pageSize,
            TotalSize = totalCount,
            PagesCount = pagesCount,
            List = profileDtos
        };

        return HandlerResult.Success(result);
    }
}
