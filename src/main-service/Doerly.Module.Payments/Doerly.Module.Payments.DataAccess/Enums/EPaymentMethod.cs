using System.ComponentModel;

namespace Doerly.Module.Payments.DataAccess.Enums;

public enum EPaymentMethod : byte
{
    [Description("Credit Card")]
    CreditCard = 1,
    
    [Description("Apple Pay")]
    ApplePay = 2,
    
    [Description("Google Pay")]
    GooglePay = 3,
    
    [Description("PayPal")]
    PayPal = 4
    
}
