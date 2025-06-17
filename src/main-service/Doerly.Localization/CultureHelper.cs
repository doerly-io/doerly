namespace Doerly.Localization;

public class CultureHelper
{
    public static void SetCurrentCulture(string culture)
    {
        ArgumentNullException.ThrowIfNull("Culture cannot be null or empty.", nameof(culture));
        
        try
        {
            var cultureInfo = new System.Globalization.CultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }
        catch (System.Globalization.CultureNotFoundException)
        {
            throw new ArgumentException($"The culture '{culture}' is not supported.", nameof(culture));
        }
        
    }
    
}
