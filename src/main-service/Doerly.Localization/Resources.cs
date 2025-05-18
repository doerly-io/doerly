using System.Resources;

namespace Doerly.Localization
{
    public static class Resources
    {
        public static ResourceManager ResourceManager { get; private set; } = new ("Doerly.Localization.Resources", typeof(Resources).Assembly);

        public static string Get(string key)
        {
            var resourceValue = ResourceManager.GetString(key);
            return resourceValue ?? key;
        }
        

    }
}
