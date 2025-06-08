using Doerly.Module.Payments.Enums;

namespace Doerly.Module.Statistics.Contracts.Dtos;

public class PaymentStatisticsDto
{
    // Payment Volume Metrics
    public decimal TotalPaymentVolume { get; set; }
    public decimal AveragePaymentAmount { get; set; }
    public Dictionary<ECurrency, decimal> PaymentVolumeByCurrency { get; set; }
    public Dictionary<DateTime, decimal> PaymentVolumeTrend { get; set; }
    
    // Payment Status Analytics
    public Dictionary<EPaymentStatus, int> PaymentStatusDistribution { get; set; }
    
    // Bill Analytics
    public decimal TotalOutstandingAmount { get; set; }
    public int TotalOutstandingBills { get; set; }
    public decimal AverageBillAmount { get; set; }
    public Dictionary<ECurrency, decimal> OutstandingAmountByCurrency { get; set; }
}