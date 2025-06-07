using Microsoft.AspNetCore.Http;

namespace Doerly.Module.Communication.Contracts.Requests;

public class SendFileMessageRequest
{
    public required IFormFile File { get; set; }
}