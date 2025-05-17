using Doerly.Domain.Models;
using Doerly.FileRepository;
using Doerly.Localization;
using Doerly.Module.Profile.DataAccess;
using Doerly.Module.Profile.Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Domain.Handlers;

public class DeleteProfileCvHandler : BaseProfileHandler
{
    private readonly IFileRepository _fileRepository;
    
    public DeleteProfileCvHandler(ProfileDbContext dbContext, IFileRepository fileRepository) : base(dbContext)
    {
        _fileRepository = fileRepository;
    }
    
    public async Task<HandlerResult> HandleAsync(int userId, CancellationToken cancellationToken = default)
    {
        var profile = await DbContext.Profiles
            .Select(x => new { x.UserId, x.CvPath })
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        
        if (profile == null)
            return HandlerResult.Failure(Resources.Get("ProfileNotFound"));
    
        if (string.IsNullOrEmpty(profile.CvPath))
            return HandlerResult.Success();
        
        var fileDeleteTask = _fileRepository.DeleteFileIfExistsAsync(
            AzureStorageConstants.DocumentsContainerName, 
            profile.CvPath);
        
        var dbUpdateTask = DbContext.Profiles
            .Where(x => x.UserId == userId)
            .ExecuteUpdateAsync(s => 
                    s.SetProperty(b => b.CvPath, (string)null),
                cancellationToken);
            
        await Task.WhenAll(fileDeleteTask, dbUpdateTask);
    
        return HandlerResult.Success();
    }
    
}