using Doerly.Domain.Models;
using Doerly.Module.Profile.DataTransferObjects;

namespace Doerly.Module.Profile.Api.ModuleWrapper;

public interface IProfileModuleWrapper
{
    Task<OperationResult<ProfileDto>> GetProfileAsync(int userId);
    Task<OperationResult<IEnumerable<ProfileDto>>> GetProfilesAsync(int[] userIds);
    Task<IEnumerable<ProfileShortInfoWithAvatarDto>> GetProfilesShortInfoWithAvatarAsync(IEnumerable<int> userIds);
}
