using Doerly.Domain.Handlers;
using Doerly.Module.Profile.DataAccess;
using ProfileModel = Doerly.Module.Profile.DataAccess.Models.Profile;
using Doerly.Module.Profile.Domain.Dtos;

namespace Doerly.Module.Profile.Domain.Handlers;

public class BaseProfileHandler(ProfileDbContext dbContext) : BaseHandler<ProfileDbContext>(dbContext)
{
    protected void CopyToProfileFromDto(ProfileModel profile, ProfileSaveDto dto)
    {
        profile.FirstName = dto.FirstName;
        profile.LastName = dto.LastName;
        profile.UserId = dto.UserId;
        profile.Patronymic = dto.Patronymic;
        profile.DateOfBirth = dto.DateOfBirth;
        profile.Sex = dto.Sex;
        profile.Bio = dto.Bio;
    }
}