using Doerly.Domain.Models;
using Doerly.Module.Profile.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers;

public class DeleteLanguageHandler(ProfileDbContext dbContext) : BaseProfileHandler(dbContext)
{
    public async Task<HandlerResult> HandleAsync(int id)
    {
        await DbContext.Languages
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();
        return HandlerResult.Success();
    }
}