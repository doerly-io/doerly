using Doerly.Domain.Models;
using Doerly.FileRepository;
using Doerly.Localization;
using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.Domain.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers;

public class UploadProfileImageHandler(ProfileDbContext dbContext, IFileRepository fileRepository) : BaseProfileHandler(dbContext)
{
    
    public async Task<HandlerResult> HandleAsync(int userId, UploadProfileImageDto dto)
    {
        var profile = await DbContext.Profiles.FirstOrDefaultAsync(x => x.UserId == userId);
        if (profile == null)
            return HandlerResult.Failure(Resources.Get("ProfileNotFound"));
        
        // Define blob storage path
        var containerName = "images";
        var imageFolderName = "ProfileImages";
        
        // TODO: Temp chatgpt suggestion. Rewrite extension check and logic to use the file extension from the uploaded file
        string fileExtension = Path.GetExtension(dto.Image.FileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(fileExtension) || !new[] { ".jpg", ".jpeg", ".png" }.Contains(fileExtension))
        {
            fileExtension = ".jpg";
        }
        
        var imageName = $"{Guid.NewGuid():N}{fileExtension}";
        var imagePath = $"{imageFolderName}/{imageName}";
        
        using (var imageStream = new MemoryStream())
        {
            await dto.Image.CopyToAsync(imageStream);
            var fileContent = imageStream.ToArray();
            await fileRepository.UploadFileAsync(containerName, imagePath, fileContent);
        }
        
        var oldImagePath = profile.ImagePath;
        if (!string.IsNullOrEmpty(oldImagePath))
        {
            await fileRepository.DeleteFileIfExistsAsync(containerName, oldImagePath);
        }
        
        profile.ImagePath = imagePath;
        await DbContext.SaveChangesAsync();
        
        return HandlerResult.Success();
    }
    
}