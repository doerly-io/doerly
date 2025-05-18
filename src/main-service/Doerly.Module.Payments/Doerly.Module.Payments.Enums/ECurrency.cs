using System.ComponentModel;

namespace Doerly.Module.Payments.Enums;

public enum ECurrency : byte
{
    [Description("UAH")]
    UAH = 1,
    
    [Description("USD")]
    USD = 2,
    
    [Description("EUR")]
    EUR = 3,
}
