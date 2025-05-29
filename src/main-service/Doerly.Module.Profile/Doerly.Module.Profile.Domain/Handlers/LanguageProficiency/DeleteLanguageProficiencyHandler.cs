using Doerly.Domain.Models;
using Doerly.Module.Profile.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers;

public class DeleteLanguageProficiencyHandler(ProfileDbContext dbContext) : BaseProfileHandler(dbContext) 
{
    public async Task<HandlerResult> HandleAsync(int userId, int id) {
        await DbContext.LanguageProficiencies
            .Where(x => x.Profile.UserId == userId && x.Id == id)
            .ExecuteDeleteAsync();
        return HandlerResult.Success();
    }
}