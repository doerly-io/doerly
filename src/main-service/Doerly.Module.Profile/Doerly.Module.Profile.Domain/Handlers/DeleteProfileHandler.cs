using Doerly.Domain.Models;
using Doerly.Module.Profile.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers;

public class DeleteProfileHandler(ProfileDbContext dbContext) : BaseProfileHandler(dbContext)
{
    public async Task<OperationResult> HandleAsync(int userId)
    {
        await DbContext.Profiles.Where(x => x.UserId == userId).ExecuteDeleteAsync();
        return OperationResult.Success();
    }
}