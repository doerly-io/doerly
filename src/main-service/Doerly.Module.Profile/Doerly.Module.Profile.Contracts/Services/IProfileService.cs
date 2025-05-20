using Doerly.Module.Profile.Contracts.Dtos;

namespace Doerly.Module.Profile.Contracts.Services;

public interface IProfileService
{
    Task<ProfileDto> GetProfileAsync(int userId, CancellationToken cancellationToken = default);
}