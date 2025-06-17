using System.Net.Mail;

namespace Doerly.Common.Settings;

public class SendGridSettings
{
    public const string SendGridSettingsName = nameof(SendGridSettings);
    
    
    public string ApiKey { get; set; }
    
    public string SenderEmail { get; set; }
    
}
