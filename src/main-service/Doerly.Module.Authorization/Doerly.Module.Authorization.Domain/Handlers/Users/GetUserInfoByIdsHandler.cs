using Doerly.Domain.Handlers;
using Doerly.Domain.Models;
using Doerly.Module.Authorization.DataTransferObjects.Responses;
using Doerly.Module.Authorization.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Authorization.Domain.Handlers;

public class GetUserInfoByIdsHandler: BaseHandler
{
    private readonly AuthorizationDbContext _context;

    public GetUserInfoByIdsHandler(AuthorizationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<UserItemResponse>> HandleAsync(IEnumerable<int> userIds)
    {
        var users = await _context.Users
            .Where(x => userIds.Contains(x.Id))
            .Select(x => new UserItemResponse
            {
                UserId = x.Id,
                Email = x.Email,
                IsEnabled = x.IsEnabled,
                IsEmailVerified = x.IsEmailVerified,
                RoleId = x.Role.Id,
                RoleName = x.Role.Name
            })
            .ToListAsync();

        return users;
    }
    
}