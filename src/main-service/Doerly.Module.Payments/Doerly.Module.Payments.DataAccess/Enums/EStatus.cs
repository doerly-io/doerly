using System.ComponentModel;

namespace Doerly.Module.Payments.DataAccess.Enums;

public enum EStatus : byte
{
    [Description("Pending")]
    Pending = 1,
    
    [Description("Approved")]
    Approved = 2,
    
    [Description("Declined")]
    Declined = 3,
    
    [Description("Canceled")]
    Canceled = 4,
    
    [Description("Refunded")]
    Refunded = 5,
    
    [Description("Error")]
    Error = 6,
    
    [Description("Expired")]
    Expired = 7,
    
    [Description("Completed")]
    Completed = 8
}
