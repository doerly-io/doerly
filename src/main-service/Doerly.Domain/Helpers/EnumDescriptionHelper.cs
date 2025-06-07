namespace Doerly.Domain.Helpers;

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
    
    
}
