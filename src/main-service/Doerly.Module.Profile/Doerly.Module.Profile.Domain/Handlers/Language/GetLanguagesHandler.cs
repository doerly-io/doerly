using System.Linq.Expressions;
using Doerly.DataTransferObjects.Pagination;
using Doerly.Domain.Models;
using Doerly.Extensions;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers;

public class GetLanguagesHandler(ProfileDbContext dbContext) : BaseProfileHandler(dbContext)
{
    public async Task<HandlerResult<PageDto<LanguageDto>>> HandleAsync(LanguagesQueryDto dto, CancellationToken cancellationToken = default)
    {
        var predicates = new List<Expression<Func<Language, bool>>>();
        
        if (!string.IsNullOrWhiteSpace(dto.Name))
        {
            var nameFilter = dto.Name.Trim().ToLower();
            predicates.Add(l => l.Name.ToLower().Contains(nameFilter) || l.Code.ToLower().Contains(nameFilter));
        }
        
        var (languages, totalCount) = await DbContext.Languages
            .AsNoTracking()
            .OrderBy(l => l.Name)
            .GetEntitiesWithPaginationAsync(dto, predicates, cancellationToken: cancellationToken);
        
        var languageDtos = languages.Select(l => new LanguageDto
        {
            Id = l.Id,
            Name = l.Name,
            Code = l.Code
        }).ToList();
        
        var pageSize = dto.Size > 0 ? dto.Size : 10; 
        var pagesCount = (int)Math.Ceiling((double)totalCount / pageSize);
        
        var result = new PageDto<LanguageDto>
        {
            PageSize = pageSize,
            TotalSize = totalCount,
            PagesCount = pagesCount,
            List = languageDtos
        };
        
        return HandlerResult.Success(result);
    }
}