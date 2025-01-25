namespace Doerly.Module.Authorization.Domain.Dtos;

public record LoginResultDto
{
    public string AccessToken { get; init; }

    public string UserEmail { get; init; }
}
