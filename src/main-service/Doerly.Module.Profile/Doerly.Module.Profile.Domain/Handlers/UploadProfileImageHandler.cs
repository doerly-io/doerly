using Doerly.Domain.Models;
using Doerly.FileRepository;
using Doerly.Localization;
using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.Domain.Constants;
using Doerly.Module.Profile.Domain.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers;

public class UploadProfileImageHandler : BaseProfileHandler
{
    private readonly IFileRepository _fileRepository;

    public UploadProfileImageHandler(ProfileDbContext dbContext, IFileRepository fileRepository) : base(dbContext)
    {
        _fileRepository = fileRepository;
    }

    public async Task<HandlerResult> HandleAsync(int userId, byte[] fileBytes)
    {
        var profile = await DbContext.Profiles.FirstOrDefaultAsync(x => x.UserId == userId);
        if (profile == null)
            return HandlerResult.Failure(Resources.Get("ProfileNotFound"));

        if (!ImageValidationHelper.IsValidImage(fileBytes, out var fileExtension))
            return HandlerResult.Failure(Resources.Get("InvalidImage"));

        var imageName = Guid.NewGuid().ToString();
        var imagePath = $"{AzureStorageConstants.FolderNames.ProfileImages}/{imageName}";
        await _fileRepository.UploadFileAsync(AzureStorageConstants.ImagesContainerName, imagePath, fileBytes);
        
        var oldImagePath = profile.ImagePath;
        if (!string.IsNullOrEmpty(oldImagePath))
            await _fileRepository.DeleteFileIfExistsAsync(AzureStorageConstants.ImagesContainerName, oldImagePath);

        profile.ImagePath = imagePath;
        await DbContext.SaveChangesAsync();

        return HandlerResult.Success();
    }
}
