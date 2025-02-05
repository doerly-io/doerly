using Doerly.Domain.Models;
using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.Domain.Dtos;
using Doerly.Module.Profile.Localization;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers;

public class GetProfileHandler(ProfileDbContext dbContext) : BaseProfileHandler(dbContext)
{
    public async Task<HandlerResult<ProfileDto>> HandleAsync(int userId)
    {
        var profile = await DbContext.Profiles
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => new
            {
                x.Id,
                x.FirstName,
                x.LastName,
                x.Patronymic,
                x.DateOfBirth,
                x.Sex,
                x.Bio,
                x.DateCreated,
                x.LastModifiedDate
            })
            .FirstOrDefaultAsync();

        if (profile == null)
            return HandlerResult.Failure<ProfileDto>(Resources.Get("ProfileNotFound"));

        var profileDto = new ProfileDto
        {
            Id = profile.Id,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            Patronymic = profile.Patronymic,
            DateOfBirth = profile.DateOfBirth,
            Sex = profile.Sex,
            Bio = profile.Bio,
            DateCreated = profile.DateCreated,
            LastModifiedDate = profile.LastModifiedDate
        };

        return HandlerResult.Success(profileDto);
    }
}