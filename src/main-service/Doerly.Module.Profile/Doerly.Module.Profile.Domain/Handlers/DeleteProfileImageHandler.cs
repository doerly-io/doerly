using Doerly.Domain.Models;
using Doerly.FileRepository;
using Doerly.Localization;
using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers;

public class DeleteProfileImageHandler : BaseProfileHandler
{
    private readonly IFileRepository _fileRepository;
    
    public DeleteProfileImageHandler(ProfileDbContext dbContext, IFileRepository fileRepository) : base(dbContext)
    {
        _fileRepository = fileRepository;
    }
    
    public async Task<OperationResult> HandleAsync(int userId, CancellationToken cancellationToken = default)
    {
        var profile = await DbContext.Profiles
            .Select(x => new { x.UserId, x.ImagePath })
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        
        if (profile == null)
            return OperationResult.Failure(Resources.Get("ProfileNotFound"));
    
        if (string.IsNullOrEmpty(profile.ImagePath))
            return OperationResult.Success();

        var fileDeleteTask = _fileRepository.DeleteFileIfExistsAsync(
            AzureStorageConstants.ImagesContainerName, 
            profile.ImagePath);
        
        var dbUpdateTask = DbContext.Profiles
            .Where(x => x.UserId == userId)
            .ExecuteUpdateAsync(s => 
                    s.SetProperty(b => b.ImagePath, (string)null),
                cancellationToken);
            
        await Task.WhenAll(fileDeleteTask, dbUpdateTask);
    
        return OperationResult.Success();
    }
    
}