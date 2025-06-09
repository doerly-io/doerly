using Doerly.Domain.Factories;
using Doerly.Domain.Models;
using Doerly.Module.Profile.DataTransferObjects;
using Doerly.Module.Profile.Domain.Handlers;

namespace Doerly.Module.Profile.Api.ModuleWrapper;

public class ProfileModuleWrapper : IProfileModuleWrapper
{
    private readonly IHandlerFactory _handlerFactory;

    public ProfileModuleWrapper(IHandlerFactory handlerFactory)
    {
        _handlerFactory = handlerFactory;
    }

    public Task<OperationResult<ProfileDto>> GetProfileAsync(int userId)
    {
        var profileResponse = _handlerFactory.Get<GetProfileHandler>().HandleAsync(userId);
        return profileResponse;
    }

    public Task<OperationResult<IEnumerable<ProfileDto>>> GetProfilesAsync(int[] userIds)
    {
        var profilesResponse = _handlerFactory.Get<GetProfilesHandler>().HandleAsync(userIds);
        return profilesResponse;
    }
}
