using System.Resources;

using Doerly.Domain.Helpers;

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
        

    }
}
