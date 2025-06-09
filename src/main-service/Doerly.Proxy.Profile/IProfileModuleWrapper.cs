using Doerly.Domain.Models;
using Doerly.Module.Profile.Contracts.Dtos;

namespace Doerly.Proxy.Profile;

public interface IProfileModuleWrapper
{
    Task<HandlerResult<ProfileDto>> GetProfileAsync(int userId);
    Task<HandlerResult<IEnumerable<ProfileDto>>> GetProfilesAsync(int[] userIds);
    
    Task<IEnumerable<ProfileShortInfoWithAvatarDto>> GetProfilesShortInfoWithAvatarAsync(IEnumerable<int> userIds);
}
