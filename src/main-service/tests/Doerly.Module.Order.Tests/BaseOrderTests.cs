using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.Enums;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using OrderEntity = Doerly.Module.Order.DataAccess.Entities.Order;
namespace Doerly.Module.Order.Tests;
public class BaseOrderTests : IClassFixture<PostgresTestContainerFixture>
{
    protected OrderDbContext OrderDbContext { get; private set; }

    public BaseOrderTests(PostgresTestContainerFixture fixture)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:OrderConnection"] = fixture.ConnectionString
            })
            .Build();

        OrderDbContext = new OrderDbContext(configuration);
        OrderDbContext.Database.Migrate();
    }

    public async Task<OrderEntity> AddTestOrderAsync()
    {
        var order = new OrderEntity
        {
            Name = "Test Order",
            Description = "Test Description",
            Price = 100.0m,
            CategoryId = 1,
            PaymentKind = EPaymentKind.Online,
            DueDate = DateTime.UtcNow.AddDays(7),
            IsPriceNegotiable = false,
            CustomerId = 123,
            Status = EOrderStatus.Placed,
        };

        OrderDbContext.Orders.Add(order);
        await OrderDbContext.SaveChangesAsync();
        return order;
    }
}