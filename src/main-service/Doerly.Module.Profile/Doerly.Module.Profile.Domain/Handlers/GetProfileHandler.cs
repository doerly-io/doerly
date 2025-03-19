using Doerly.Domain.Models;
using Doerly.FileRepository;
using Doerly.Localization;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers;

public class GetProfileHandler(ProfileDbContext dbContext, IFileRepository fileRepository) : BaseProfileHandler(dbContext)
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
                x.DateOfBirth,
                x.Sex,
                x.Bio,
                x.DateCreated,
                x.LastModifiedDate,
                x.ImagePath
            })
            .FirstOrDefaultAsync();

        if (profile == null)
            return HandlerResult.Failure<ProfileDto>(Resources.Get("ProfileNotFound"));

        string imageUrl = null;
        if (!string.IsNullOrEmpty(profile.ImagePath))
        {
            imageUrl = await fileRepository.GetSasUrlAsync("images", profile.ImagePath);
        }
        
        var profileDto = new ProfileDto
        {
            Id = profile.Id,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            DateOfBirth = profile.DateOfBirth,
            Sex = profile.Sex,
            Bio = profile.Bio,
            DateCreated = profile.DateCreated,
            LastModifiedDate = profile.LastModifiedDate,
            ImageUrl = imageUrl
        };

        return HandlerResult.Success(profileDto);
    }
}
