using Doerly.Domain.Models;
using Doerly.Module.Profile.DataTransferObjects;
using Doerly.Proxy.BaseProxy;

namespace Doerly.Proxy.Profile;

public interface IProfileModuleProxy : IModuleProxy
{
    Task<OperationResult<ProfileDto>> GetProfileAsync(int userId);
    Task<OperationResult<IEnumerable<ProfileDto>>> GetProfilesAsync(int[] userIds);
}