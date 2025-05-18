namespace Doerly.Module.Payments.Enums;

public enum EPaymentStatus
{
    /// <summary>
    /// The payment request has been created but not yet processed.
    /// </summary>
    Pending = 1,
    
    /// <summary>
    /// The payment was not completed successfully.
    /// </summary>
    Failed = 2,
    
    /// <summary>
    /// Payment request data is incomplete or invalid.
    /// </summary>
    Error = 3,
    
    /// <summary>
    /// The payment was completed successfully.
    /// </summary>
    Completed = 10,
}
