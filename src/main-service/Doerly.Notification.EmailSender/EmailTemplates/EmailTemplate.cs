using System.Resources;

namespace Doerly.Notification.EmailSender;

public static class EmailTemplate
{
    public const string ResetPassword = nameof(ResetPassword);
    
    public const string EmailVerification = nameof(EmailVerification);
    
    private static readonly ResourceManager _resourceManager = new(typeof(EmailTemplate).FullName!, typeof(EmailTemplate).Assembly);

    public static string Get(string key)
    {
        var resourceValue = _resourceManager.GetString(key);
        return resourceValue ?? key;
    }
}
