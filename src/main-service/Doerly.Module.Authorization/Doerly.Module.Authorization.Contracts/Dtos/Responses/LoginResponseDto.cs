namespace Doerly.Module.Authorization.Contracts.Dtos;

public record LoginResponseDto
{
    public string AccessToken { get; init; }

    public string UserEmail { get; init; }
}
