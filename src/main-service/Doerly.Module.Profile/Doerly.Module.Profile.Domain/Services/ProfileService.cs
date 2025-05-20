using Doerly.Domain.Factories;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.Contracts.Services;
using Doerly.Module.Profile.Domain.Handlers;

namespace Doerly.Module.Profile.Domain.Services;

public class ProfileService : IProfileService
{
    private readonly IHandlerFactory _handlerFactory;

    public ProfileService(IHandlerFactory handlerFactory)
    {
        _handlerFactory = handlerFactory;
    }

    public async Task<ProfileDto> GetProfileAsync(int userId, CancellationToken cancellationToken = default)
    {
        var handler = _handlerFactory.Get<GetProfileHandler>();
        var result = await handler.HandleAsync(userId, cancellationToken);

        if (!result.IsSuccess)
        {
            throw new Exception(result.ErrorMessage); //TODO: Handle error properly
        }

        return result.Value;
    }
}