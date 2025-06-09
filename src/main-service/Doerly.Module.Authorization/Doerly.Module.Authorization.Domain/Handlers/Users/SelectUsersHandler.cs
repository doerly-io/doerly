using Doerly.DataTransferObjects.Pagination;
using Doerly.Domain.Handlers;
using Doerly.Extensions;
using Doerly.Module.Authorization.DataTransferObjects.Responses;
using Doerly.Module.Authorization.DataAccess;
using Doerly.Module.Authorization.DataAccess.Entities;

namespace Doerly.Module.Authorization.Domain.Handlers;

public class SelectUsersHandler : BaseHandler
{
    private readonly AuthorizationDbContext _context;

    public SelectUsersHandler(AuthorizationDbContext context)
    {
        _context = context;
    }

    public async Task<BasePaginationResponse<UserItemResponse>> HandleAsync(
        GetEntitiesWithPaginationRequest paginationRequest)
    {
        var users = await _context.Users
            .GetEntitiesWithPaginationAsync(paginationRequest.PageInfo, selector: x => new UserEntity
            {
                Email = x.Email,
                IsEnabled = x.IsEnabled,
                IsEmailVerified = x.IsEmailVerified,
                Role = new RoleEntity()
                {
                    Id = x.Role.Id,
                    Name = x.Role.Name,
                }
            });

        var result = new BasePaginationResponse<UserItemResponse>()
        {
            Count = users.TotalCount,
            Items = users.Entities.Select(x => new UserItemResponse()
            {
                Email = x.Email,
                IsEnabled = x.IsEnabled,
                IsEmailVerified = x.IsEmailVerified,
                RoleId = x.Role?.Id,
                RoleName = x.Role?.Name
            })
        };

        return result;
    }
}