using Doerly.Domain.Handlers;
using Doerly.Module.Payments.DataTransferObjects.Responses;
using Doerly.Module.Payments.DataAccess;
using Doerly.Module.Payments.DataAccess.Models;
using Doerly.Module.Payments.Enums;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Payments.Domain.Handlers;

public class GetPaymentStatisticsHandler : BaseHandler<PaymentDbContext>
{
    public GetPaymentStatisticsHandler(PaymentDbContext dbContext) : base(dbContext)
    {
    }
    
    public async Task<PaymentStatisticsDto> HandleAsync()
    {
        var utcNow = DateTime.UtcNow;
        
        // Get all payments and bills for analysis
        var payments = await DbContext.Set<Payment>()
            .Include(p => p.Bill)
            .ToListAsync();
            
        var bills = await DbContext.Set<Bill>()
            .Include(b => b.Payments)
            .ToListAsync();

        // Payment Volume Metrics
        var completedPayments = payments
            .Where(p => p.Status == EPaymentStatus.Completed)
            .ToList();
        
        var totalPaymentVolume = completedPayments
            .Sum(p => p.Amount);
        
        var averagePaymentAmount = completedPayments.Any() 
            ? completedPayments.Average(p => p.Amount) 
            : 0;

        // Payment volume by currency
        var paymentVolumeByCurrency = completedPayments
            .GroupBy(p => p.Currency)
            .ToDictionary(g => g.Key, g => g.Sum(p => p.Amount));

        // Payment volume trend (last 30 days by day)
        var paymentVolumeTrend = new Dictionary<DateTime, decimal>();
        for (int i = 0; i < 30; i++)
        {
            var date = utcNow.AddDays(-i).Date;
            var volumeForDate = completedPayments
                .Where(p => p.DateCreated.Date == date)
                .Sum(p => p.Amount);
            paymentVolumeTrend[date] = volumeForDate;
        }

        // Payment Status Distribution
        var paymentStatusDistribution = payments
            .GroupBy(p => p.Status)
            .ToDictionary(g => g.Key, g => g.Count());

        // Bill Analytics
        var outstandingBills = bills
            .Where(b => b.AmountPaid < b.AmountTotal)
            .ToList();

        var totalOutstandingAmount = outstandingBills
            .Sum(b => b.AmountTotal - b.AmountPaid);

        var totalOutstandingBills = outstandingBills.Count;

        var averageBillAmount = bills.Any() 
            ? bills.Average(b => b.AmountTotal) 
            : 0;

        // Outstanding amount by currency (assuming bills have currency property or we derive from payments)
        var outstandingAmountByCurrency = new Dictionary<ECurrency, decimal>();
        
        // Group outstanding amounts by currency based on the payments associated with each bill
        foreach (var bill in outstandingBills)
        {
            var outstandingForBill = bill.AmountTotal - bill.AmountPaid;
            
            // Get the currency from the bill's payments, use the most recent payment's currency
            var billCurrency = bill.Payments
                .OrderByDescending(p => p.DateCreated)
                .FirstOrDefault()?.Currency ?? ECurrency.USD; // Default to USD if no payments
            
            if (outstandingAmountByCurrency.ContainsKey(billCurrency))
            {
                outstandingAmountByCurrency[billCurrency] += outstandingForBill;
            }
            else
            {
                outstandingAmountByCurrency[billCurrency] = outstandingForBill;
            }
        }

        return new PaymentStatisticsDto
        {
            // Payment Volume Metrics
            TotalPaymentVolume = totalPaymentVolume,
            AveragePaymentAmount = averagePaymentAmount,
            PaymentVolumeByCurrency = paymentVolumeByCurrency,
            PaymentVolumeTrend = paymentVolumeTrend,
            
            // Payment Status Analytics
            PaymentStatusDistribution = paymentStatusDistribution,
            
            // Bill Analytics
            TotalOutstandingAmount = totalOutstandingAmount,
            TotalOutstandingBills = totalOutstandingBills,
            AverageBillAmount = averageBillAmount,
            OutstandingAmountByCurrency = outstandingAmountByCurrency
        };

    }
}
