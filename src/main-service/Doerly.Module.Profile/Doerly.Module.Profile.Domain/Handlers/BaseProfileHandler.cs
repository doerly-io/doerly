using Doerly.Domain.Handlers;
using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.DataAccess.Models;
using Doerly.Module.Profile.Domain.Dtos;

namespace Doerly.Module.Profile.Domain.Handlers;

public class BaseProfileHandler(ProfileDbContext dbContext) : BaseHandler<ProfileDbContext>(dbContext)
{
    protected void CopyFromDto(ProfileEntity profileEntity, ProfileSaveDto dto)
    {
        profileEntity.FirstName = dto.FirstName;
        profileEntity.LastName = dto.LastName;
        profileEntity.UserId = dto.UserId;
        profileEntity.DateOfBirth = dto.DateOfBirth;
        profileEntity.Sex = dto.Sex;
        profileEntity.Bio = dto.Bio;
    }
}