using System.Resources;

namespace Doerly.Localization;

public static class Resources
{
    private static readonly ResourceManager _resourceManager = new(typeof(Resources).FullName, typeof(Resources).Assembly);
    
    public static string Get(string key)
    {
        var resourceValue = _resourceManager.GetString(key);
        return resourceValue ?? key;
    }
}
