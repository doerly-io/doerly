using Doerly.Domain.Models;
using Doerly.FileRepository;
using Doerly.Localization;
using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.Domain.Constants;
using Doerly.Module.Profile.Domain.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers;

public class UploadProfileCvHandler : BaseProfileHandler
{
    private readonly IFileRepository _fileRepository;
    
    public UploadProfileCvHandler(ProfileDbContext dbContext, IFileRepository fileRepository) : base(dbContext)
    {
        _fileRepository = fileRepository;
    }
    
    public async Task<OperationResult> HandleAsync(int userId, byte[] fileBytes, CancellationToken cancellationToken = default)
    {
        if (!DocumentValidationHelper.IsValidDocument(fileBytes, out var fileExtension))
            return OperationResult.Failure(Resources.Get("InvalidDocument"));
        
        var profile = await DbContext.Profiles
            .Select(x => new { x.UserId, x.CvPath })
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        
        if (profile == null)
            return OperationResult.Failure(Resources.Get("ProfileNotFound"));
    
        var documentName = Guid.NewGuid().ToString();
        var documentPath = $"{AzureStorageConstants.FolderNames.ProfileCvs}/{documentName}{fileExtension}";
    
        var tasks = new List<Task>(2);
    
        tasks.Add(_fileRepository.UploadFileAsync(
            AzureStorageConstants.DocumentsContainerName, 
            documentPath, 
            fileBytes));
    
        if (!string.IsNullOrEmpty(profile.CvPath))
        {
            tasks.Add(_fileRepository.DeleteFileIfExistsAsync(
                AzureStorageConstants.DocumentsContainerName, 
                profile.CvPath));
        }
    
        await Task.WhenAll(tasks);
    
        await DbContext.Profiles
            .Where(x => x.UserId == userId)
            .ExecuteUpdateAsync(s => 
                s.SetProperty(b => b.CvPath, documentPath),
                cancellationToken);
    
        return OperationResult.Success();
    }
}