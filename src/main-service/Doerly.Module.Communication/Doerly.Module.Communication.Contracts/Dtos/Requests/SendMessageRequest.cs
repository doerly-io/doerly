using System.Text.Json.Serialization;

namespace Doerly.Module.Communication.Contracts.Dtos.Requests;

public class SendMessageRequest
{
    public required int RecipientId { get; set; }
    
    public required string MessageContent { get; set; }
    
    [JsonIgnore]
    public int InitiatorId { get; set; }
}