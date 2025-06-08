using Doerly.Domain.Models;
using Doerly.Module.Profile.Api.ModuleWrapper;
using Doerly.Module.Profile.Contracts.Dtos;

namespace Doerly.Proxy.Profile;

public class ProfileModuleProxy : IProfileModuleProxy
{
    private readonly IProfileModuleWrapper _profileModuleWrapper;
    
    public ProfileModuleProxy(IProfileModuleWrapper profileModuleWrapper)
    {
        _profileModuleWrapper = profileModuleWrapper;
    }
    
    public Task<HandlerResult<ProfileDto>> GetProfileAsync(int userId)
    {
        var profileResponse = _profileModuleWrapper.GetProfileAsync(userId);
        return profileResponse;
    }

    public Task<HandlerResult<IEnumerable<ProfileDto>>> GetProfilesAsync(int[] userIds)
    {
        var profilesResponse = _profileModuleWrapper.GetProfilesAsync(userIds);
        return profilesResponse;
    }

    public Task<IEnumerable<ProfileShortInfoWithAvatarDto>> GetProfilesShortInfoWithAvatarAsync(IEnumerable<int> userIds)
    {
        var profilesShortResponse = _profileModuleWrapper.GetProfilesShortInfoWithAvatarAsync(userIds);
        return profilesShortResponse;
    }
}
