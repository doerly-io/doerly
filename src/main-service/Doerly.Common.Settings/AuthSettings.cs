namespace Doerly.Common.Settings;

public class AuthSettings
{
    public const string AuthSettingsName = nameof(AuthSettings);
    
    public string SecretKey { get; set; }
    
    public string Issuer { get; set; }
    
    public string Audience { get; set; }
    
    /// <summary>
    /// Token lifetime in minutes
    /// </summary>
    public int AccessTokenLifetime { get; set; }

    /// <summary>
    /// Token lifetime in minutes
    /// </summary>
    public int RefreshTokenLifetime { get; set; }

    /// <summary>
    /// Token lifetime in minutes
    /// </summary>
    public int PasswordResetTokenLifetime { get; set; }
    
}
