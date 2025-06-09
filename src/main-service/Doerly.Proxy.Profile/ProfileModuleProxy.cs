using Doerly.Domain.Models;
using Doerly.Module.Profile.Api.ModuleWrapper;
using Doerly.Module.Profile.DataTransferObjects;

namespace Doerly.Proxy.Profile;

public class ProfileModuleProxy : IProfileModuleProxy
{
    private readonly IProfileModuleWrapper _profileModuleWrapper;
    
    public ProfileModuleProxy(IProfileModuleWrapper profileModuleWrapper)
    {
        _profileModuleWrapper = profileModuleWrapper;
    }
    
    public Task<OperationResult<ProfileDto>> GetProfileAsync(int userId)
    {
        var profileResponse = _profileModuleWrapper.GetProfileAsync(userId);
        return profileResponse;
    }

    public Task<OperationResult<IEnumerable<ProfileDto>>> GetProfilesAsync(int[] userIds)
    {
        var profilesResponse = _profileModuleWrapper.GetProfilesAsync(userIds);
        return profilesResponse;
    }
}
