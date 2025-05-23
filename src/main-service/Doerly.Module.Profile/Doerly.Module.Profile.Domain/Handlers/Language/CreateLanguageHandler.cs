using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers;

public class CreateLanguageHandler(ProfileDbContext dbContext) : BaseProfileHandler(dbContext) 
{
    public async Task<HandlerResult> HandleAsync(LanguageSaveDto dto, CancellationToken cancellationToken = default)
    {
        var validationResult = await ValidateLanguageIsUniqueAsync(dto.Name, dto.Code, cancellationToken: cancellationToken);
        
        if (!validationResult.IsSuccess)
            return validationResult;

        var newLanguage = new Language
        {
            Name = dto.Name,
            Code = dto.Code
        };

        DbContext.Languages.Add(newLanguage);
        await DbContext.SaveChangesAsync(cancellationToken);
        return HandlerResult.Success();
    }
}