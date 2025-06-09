namespace Doerly.Module.Authorization.DataTransferObjects.Responses;

public record LoginResponseDto
{
    public string AccessToken { get; init; }

    public string UserEmail { get; init; }
}
