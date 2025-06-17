using System.ComponentModel;

namespace Doerly.Module.Payments.Enums;

public enum EPaymentMethod : byte
{
    [Description("BankCard")]
    Card = 1,
    
    [Description("Privat24")]
    Privat24 = 2,
    
    [Description("Masterpass")]
    Masterpass = 3,
    
    [Description("Cash")]
    Cash = 4,
    
    [Description("QRCode")]
    QrCode = 5,
    
    [Description("Invoice")]
    Invoice = 6,
}
