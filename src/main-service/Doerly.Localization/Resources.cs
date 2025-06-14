using System.Resources;
using Doerly.Common.Helpers;

namespace Doerly.Localization
{
    public static class Resources
    {
        public static ResourceManager ResourceManager { get; private set; } = new ("Doerly.Localization.Resources", typeof(Resources).Assembly);

        public static string Get(string key)
        {
            var resourceValue = SafeExecutor.Execute(() => ResourceManager.GetString(key));
            return resourceValue.Value ?? key;
        }
        
        public static string Get(string key, params object[] args)
        {
            var resourceValue = SafeExecutor.Execute(() => ResourceManager.GetString(key));
            return resourceValue.Value != null ? string.Format(resourceValue.Value, args) : key;
        }
    }
}
