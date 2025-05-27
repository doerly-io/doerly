using System.Diagnostics;
using Doerly.Module.Payments.DataAccess.Models;
using Doerly.Module.Payments.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Doerly.Module.Authorization.Tests;

public class Test : BasePaymentTests
{
    public Test(MsSqlTestContainerFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Test1()
    {
        // Arrange
        var bill = new Bill
        {
            Description = "Test",
            AmountTotal = 100.00m,
            PayerId = 1,
            AmountPaid = 0,
        };

        List<Payment> payments =
        [
            new()
            {
                BillId = 1,
                Amount = 100.00m,
                Status = EPaymentStatus.Pending,
                Action = EPaymentAction.Pay,
                Currency = ECurrency.UAH,
                Description = "Test",
            },
            new()
            {
                BillId = 1,
                Amount = 50.00m,
                Status = EPaymentStatus.Pending,
                Action = EPaymentAction.Pay,
                Currency = ECurrency.UAH,
                Description = "Test 2",
            }
        ];

        DbContext.Add(bill);
        DbContext.AddRange(payments);
        DbContext.SaveChanges();
        
        var billId = 1;
        
        var desc = "Test Bill Description";
        // Act
        
        var sw = new Stopwatch();
        
        sw.Start();
        
        // var spayments = await DbContext.Payments
        //     .Where(p => p.BillId == billId && p.Status == EPaymentStatus.Pending)
        //     .ToListAsync();
        
        // var selectedBill = await DbContext.Bills
        //     .Where(b => b.Id == billId)
        //     .FirstOrDefaultAsync();
        
       //  var sbill = await DbContext.Bills
       //      .Include(i => i.Payments.Where(x => x.Status == EPaymentStatus.Pending))
       //      .AsSplitQuery()
       //      .FirstOrDefaultAsync(i => i.Id == billId);
       //  
       // await DbContext.Entry(sbill).Collection(b => b.Payments).LoadAsync();
       
       var sbill = await DbContext.Bills
           .Where(i => i.Id == billId)
           .AsSplitQuery()
           .Include(i => i.Payments) // Load all payments
           .FirstOrDefaultAsync();
       
       
       // var spayments = await DbContext.Payments
       //      .Where(p => p.BillId == billId && p.Status == EPaymentStatus.Pending)
       //      .Include(p => p.Bill)
       //      .AsSplitQuery()
       //      .ToListAsync();

       // var b = await  DbContext.Bills
       //     .Where(x => x.Description == "''select ';drop database doerly;").FirstOrDefaultAsync();
        
        sw.Stop();
        Debug.WriteLine($"Time taken: {sw.ElapsedMilliseconds} ms");

        // Assert
    }
}
