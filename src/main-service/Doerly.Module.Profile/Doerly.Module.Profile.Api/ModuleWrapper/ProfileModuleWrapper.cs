using Doerly.Domain.Factories;
using Doerly.Domain.Models;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.Domain.Handlers;
using Doerly.Proxy.Profile;

namespace Doerly.Module.Profile.Api.ModuleWrapper;
   
public class ProfileModuleWrapper : IProfileModuleWrapper
{
    private readonly IHandlerFactory _handlerFactory;

    public ProfileModuleWrapper(IHandlerFactory handlerFactory)
    {
        _handlerFactory = handlerFactory;
    }

    public Task<HandlerResult<ProfileDto>> GetProfileAsync(int userId)
    {
        var profileResponse = _handlerFactory.Get<GetProfileHandler>().HandleAsync(userId);
        return profileResponse;
    }

    public Task<HandlerResult<IEnumerable<ProfileDto>>> GetProfilesAsync(int[] userIds)
    {
        var profilesResponse = _handlerFactory.Get<GetProfilesHandler>().HandleAsync(userIds);
        return profilesResponse;
    }
    
    public Task<IEnumerable<ProfileShortInfoWithAvatarDto>> GetProfilesShortInfoWithAvatarAsync(IEnumerable<int> userIds)
    {
        var profilesShortResponse = _handlerFactory.Get<GetProfilesShortInfoWithAvatars>().HandleAsync(userIds);
        return profilesShortResponse;
    }
}
