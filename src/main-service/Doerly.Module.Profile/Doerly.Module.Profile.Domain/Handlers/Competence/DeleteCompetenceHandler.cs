using Doerly.Domain.Models;
using Doerly.Module.Profile.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers;

public class DeleteCompetenceHandler(ProfileDbContext dbContext) : BaseProfileHandler(dbContext)
{
    public async Task<HandlerResult> HandleAsync(int userId, int competenceId, CancellationToken cancellationToken = default)
    {
        await DbContext.Competences
            .Where(c => c.ProfileId == userId && c.Id == competenceId)
            .ExecuteDeleteAsync(cancellationToken);
        return HandlerResult.Success();
    }
}