using Doerly.Domain;
using Doerly.Domain.Models;
using Doerly.Messaging;
using Doerly.Module.Order.Contracts.Dtos;
using Doerly.Module.Order.Domain.Handlers.Order;
using Doerly.Module.Order.Enums;
using Doerly.Module.Payments.Contracts;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Proxy.Payment;

using Moq;

namespace Doerly.Module.Order.Tests;
public class UpdateOrderStatusHandlerTests : BaseOrderTests
{
    private readonly UpdateOrderStatusHandler _handler;
    private readonly Mock<IDoerlyRequestContext> _doerlyRequestContextMock;
    private readonly Mock<IPaymentModuleProxy> _paymentModuleProxyMock;

    public UpdateOrderStatusHandlerTests(PostgresTestContainerFixture fixture) : base(fixture)
    {
        _paymentModuleProxyMock = new Mock<IPaymentModuleProxy>();
        _doerlyRequestContextMock = new Mock<IDoerlyRequestContext>();
        var messagePublisherMock = new Mock<IMessagePublisher>();

        _handler = new UpdateOrderStatusHandler(OrderDbContext, _paymentModuleProxyMock.Object,
            _doerlyRequestContextMock.Object, messagePublisherMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ShouldUpdateStatus_WhenOrderExists()
    {
        var order = await AddTestOrderAsync();
        order.PaymentKind = EPaymentKind.Cash;
        order.Status = EOrderStatus.InProgress;
        order.ExecutorId = 1;

        await OrderDbContext.SaveChangesAsync();

        _doerlyRequestContextMock.SetupProperty(x => x.UserId, order.CustomerId); 

        var request = new UpdateOrderStatusRequest
        {
            Status = EOrderStatus.Completed
        };

        var result = await _handler.HandleAsync(order.Id, request);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnFailure_WhenOrderDoesNotExist()
    {
        var request = new UpdateOrderStatusRequest 
        { 
            Status = EOrderStatus.Completed 
        };
        var result = await _handler.HandleAsync(-1, request);
        Assert.False(result.IsSuccess);
    }
}