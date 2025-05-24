using Doerly.Domain.Factories;
using Doerly.Domain.Models;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.Domain.Handlers;

namespace Doerly.Module.Profile.Api.ModuleWrapper;

public interface IProfileModuleWrapper
{
    Task<HandlerResult<ProfileDto>> GetProfileAsync(int userId);
    Task<HandlerResult<ICollection<ProfileDto>>> GetProfilesAsync(int[] userIds);
}
    
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

    public Task<HandlerResult<ICollection<ProfileDto>>> GetProfilesAsync(int[] userIds)
    {
        var profilesResponse = _handlerFactory.Get<GetProfilesHandler>().HandleAsync(userIds);
        return profilesResponse;
    }
}