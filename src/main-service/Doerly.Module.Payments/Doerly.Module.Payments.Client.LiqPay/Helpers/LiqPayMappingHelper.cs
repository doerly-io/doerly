using Doerly.Module.Payments.Enums;

namespace Doerly.Module.Payments.Client.LiqPay.Helpers;

public class LiqPayMappingHelper
{
    public static EPaymentStatus MapLiqPayStatusToCommonStatus(string liqPayStatus)
    {
        switch (liqPayStatus)
        {
            case LiqPayStatusConstants.Success:
            case LiqPayStatusConstants.Subscribed:
            case LiqPayStatusConstants.Unsubscribed:
            case LiqPayStatusConstants.Reversed:
                return EPaymentStatus.Completed;
            case LiqPayStatusConstants.Failure:
                return EPaymentStatus.Failed;
            case LiqPayStatusConstants.Error:
                return EPaymentStatus.Error;
            default:
                return EPaymentStatus.Pending;
            
        }
    }
    
    public static string MapCommonActionToLiqPayAction(EPaymentAction paymentAction)
    {
        return paymentAction switch
        {
            EPaymentAction.Pay => LiqPayActionConstants.Pay,
            EPaymentAction.Hold => LiqPayActionConstants.Hold,
            _ => throw new ArgumentOutOfRangeException(nameof(paymentAction), paymentAction, null)
        };
    }
}
