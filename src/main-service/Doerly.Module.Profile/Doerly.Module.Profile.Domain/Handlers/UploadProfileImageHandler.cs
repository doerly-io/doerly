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

    public async Task<HandlerResult> HandleAsync(int userId, byte[] fileBytes, CancellationToken cancellationToken = default)
    {
        if (!ImageValidationHelper.IsValidImage(fileBytes, out var fileExtension))
            return HandlerResult.Failure(Resources.Get("InvalidImage"));

        var profile = await DbContext.Profiles
            .Select(x => new { x.UserId, x.ImagePath })
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        
        if (profile == null)
            return HandlerResult.Failure(Resources.Get("ProfileNotFound"));
    
        var imageName = Guid.NewGuid().ToString();
        var imagePath = $"{AzureStorageConstants.FolderNames.ProfileImages}/{imageName}{fileExtension}";
    
        var tasks = new List<Task>(2);
    
        tasks.Add(_fileRepository.UploadFileAsync(
            AzureStorageConstants.ImagesContainerName, 
            imagePath, 
            fileBytes));
    
        if (!string.IsNullOrEmpty(profile.ImagePath))
        {
            tasks.Add(_fileRepository.DeleteFileIfExistsAsync(
                AzureStorageConstants.ImagesContainerName, 
                profile.ImagePath));
        }
    
        await Task.WhenAll(tasks);
    
        await DbContext.Profiles
            .Where(x => x.UserId == userId)
            .ExecuteUpdateAsync(s => 
                    s.SetProperty(b => b.ImagePath, imagePath)
                        .SetProperty(b => b.LastModifiedDate, DateTime.UtcNow),
                cancellationToken);
    
        return HandlerResult.Success();
    }
}
