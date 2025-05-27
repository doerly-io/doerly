using Doerly.Domain.Helpers;

using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Resources;

namespace Doerly.Localization
{
    public class DataAnnotationsStringLocalizer : IStringLocalizer
    {
        private readonly ResourceManager _resourceManager;

        public DataAnnotationsStringLocalizer(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        public LocalizedString this[string name]
        {
            get
            {
                var value = _resourceManager.GetString(name, CultureInfo.CurrentUICulture);
                return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = SafeExecutor.Execute(() => _resourceManager.GetString(name, CultureInfo.CurrentUICulture));
                var value = string.Format(format.Value ?? name, arguments);
                return new LocalizedString(name, value, resourceNotFound: format == null);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
