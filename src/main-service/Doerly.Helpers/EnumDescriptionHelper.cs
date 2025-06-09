using Doerly.Localization;

namespace Doerly.Helpers;

public static class EnumDescriptionHelper
{
    public static string ToDescription<TEnum>(this TEnum value) where TEnum : struct, Enum
    {
        var descriptionAttribute = value
            .GetType()
            .GetField(value.ToString())
            ?.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false)
            .FirstOrDefault() as System.ComponentModel.DescriptionAttribute;

        return descriptionAttribute?.Description ?? value.ToString();
    }

    public static string ToLocalizedDescription<TEnum>(this TEnum value) where TEnum : struct, Enum
    {
        var description = value.ToDescription();
        var result = Resources.Get(description);
        return result == description ? value.ToString() : result;
    }
}
