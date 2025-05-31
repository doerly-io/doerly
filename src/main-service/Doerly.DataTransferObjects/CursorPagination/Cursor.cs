using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

namespace Doerly.DataTransferObjects;

public record Cursor(int? LastId)
{
    public static string Encode(int? lastId)
    {
        var cursor = new Cursor(lastId);
        var json = JsonSerializer.Serialize(cursor);
        var res = Base64UrlEncoder.Encode(json);
        return res;
    }
    
    public static Cursor Decode(string? encodedCursor)
    {
        var trimmedCursor = encodedCursor?.Trim();
        if (string.IsNullOrEmpty(trimmedCursor))
            return new Cursor((int?)null);
        
        var json = Base64UrlEncoder.Decode(trimmedCursor);
        var cursor = JsonSerializer.Deserialize<Cursor>(json);
        return cursor;
    }
    
}
