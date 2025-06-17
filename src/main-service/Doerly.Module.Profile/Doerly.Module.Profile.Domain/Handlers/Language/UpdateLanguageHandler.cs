using Doerly.Domain.Models;
using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.DataTransferObjects;

namespace Doerly.Module.Profile.Domain.Handlers;

public class UpdateLanguageHandler(ProfileDbContext dbContext) : BaseProfileHandler(dbContext)
{
    public async Task<OperationResult> HandleAsync(int id, LanguageSaveDto dto, CancellationToken cancellationToken = default)
    {
        var result = await GetLanguageByIdAsync(id, cancellationToken);
        
        if (!result.IsSuccess)
            return result;
        
        var language = result.Value;
        var validationResult = await ValidateLanguageIsUniqueAsync(
            dto.Name, dto.Code, excludeId: id, cancellationToken);
        
        if (!validationResult.IsSuccess)
            return validationResult;
        
        language.Name = dto.Name;
        language.Code = dto.Code;
        
        await DbContext.SaveChangesAsync();
        
        return OperationResult.Success();
    }
}