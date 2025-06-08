using Doerly.Domain.Handlers;
using Doerly.Domain.Models;
using Doerly.Module.Statistics.Contracts.Dtos;
using Doerly.Proxy.Payment;
using MassTransit.Initializers;

namespace Doerly.Module.Statistics.Domain.Handlers;

public class GetPaymentStatisticsHandler : BaseHandler 
{
    private readonly IPaymentModuleProxy _paymentModuleProxy;
    
    public GetPaymentStatisticsHandler(IPaymentModuleProxy paymentModuleProxy)
    {
        _paymentModuleProxy = paymentModuleProxy;
    }
    
    public async Task<HandlerResult<PaymentStatisticsDto>> HandleAsync()
    {
        var statistics = await _paymentModuleProxy
            .GetPaymentStatisticsAsync()
            .Select(x => new PaymentStatisticsDto
            {
                TotalPaymentVolume = x.TotalPaymentVolume,
                AveragePaymentAmount = x.AveragePaymentAmount,
                PaymentVolumeByCurrency = x.PaymentVolumeByCurrency,
                PaymentVolumeTrend = x.PaymentVolumeTrend,
                PaymentStatusDistribution = x.PaymentStatusDistribution,
                TotalOutstandingAmount = x.TotalOutstandingAmount,
                TotalOutstandingBills = x.TotalOutstandingBills,
                AverageBillAmount = x.AverageBillAmount,
                OutstandingAmountByCurrency = x.OutstandingAmountByCurrency
            });
        return HandlerResult.Success(statistics);
    }
}