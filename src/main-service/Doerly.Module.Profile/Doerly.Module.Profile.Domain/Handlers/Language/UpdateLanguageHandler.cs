using Doerly.Domain.Models;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.DataAccess;

namespace Doerly.Module.Profile.Domain.Handlers;

public class UpdateLanguageHandler(ProfileDbContext dbContext) : BaseProfileHandler(dbContext)
{
    public async Task<HandlerResult> HandleAsync(int id, LanguageSaveDto dto, CancellationToken cancellationToken = default)
    {
        var (language, result) = await GetLanguageByIdAsync(id, cancellationToken);
        
        if (!result.IsSuccess)
            return result;
        
        var validationResult = await ValidateLanguageIsUniqueAsync(
            dto.Name, dto.Code, excludeId: id, cancellationToken);
        
        if (!validationResult.IsSuccess)
            return validationResult;
        
        language.Name = dto.Name;
        language.Code = dto.Code;
        
        await DbContext.SaveChangesAsync();
        
        return HandlerResult.Success();
    }
}