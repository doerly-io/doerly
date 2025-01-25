namespace Doerly.Common;

public class JwtSettings
{
    public const string JwtSettingsName = nameof(JwtSettings);
    
    public string SecretKey { get; set; }
    
    public string Issuer { get; set; }
    
    public string Audience { get; set; }
    
    /// <summary>
    /// Token lifetime in minutes
    /// </summary>
    public int AccessTokenExpiration { get; set; }
    
}
