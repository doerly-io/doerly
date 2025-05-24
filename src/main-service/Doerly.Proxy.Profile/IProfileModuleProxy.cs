using Doerly.Domain.Models;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Proxy.BaseProxy;

namespace Doerly.Proxy.Profile;

public interface IProfileModuleProxy : IModuleProxy
{
    Task<HandlerResult<ProfileDto>> GetProfileAsync(int userId);
    Task<HandlerResult<ICollection<ProfileDto>>> GetProfilesAsync(int[] userIds);
}