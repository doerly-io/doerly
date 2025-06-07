namespace Doerly.Module.Authorization.Contracts.Responses;

public record LoginResponseDto
{
    public string AccessToken { get; init; }

    public string UserEmail { get; init; }
}
