using Doerly.Domain;
using Doerly.Messaging;
using Doerly.Module.Order.DataTransferObjects.Requests;
using Doerly.Module.Order.Domain.Handlers;
using Doerly.Module.Order.Enums;
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
    public async Task HandleAsync_ShouldUpdateStatus_WhenUserIsCustomer()
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
        order = await OrderDbContext.Orders.FindAsync(order.Id);
        Assert.Equal(EOrderStatus.AwaitingConfirmation, order.Status);
    }

    [Fact]
    public async Task HandleAsync_ShouldUpdateStatus_WhenUserIsExecutor()
    {
        var order = await AddTestOrderAsync();
        order.PaymentKind = EPaymentKind.Online;
        order.Status = EOrderStatus.InProgress;
        order.ExecutorId = 1;

        await OrderDbContext.SaveChangesAsync();

        _doerlyRequestContextMock.SetupProperty(x => x.UserId, order.ExecutorId);

        var request = new UpdateOrderStatusRequest
        {
            Status = EOrderStatus.Completed
        };

        var result = await _handler.HandleAsync(order.Id, request);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        order = await OrderDbContext.Orders.FindAsync(order.Id);
        Assert.Equal(EOrderStatus.AwaitingPayment, order.Status);
    }

    [Fact]
    public async Task HandleAsync_ShouldUpdateStatus_WhenBothConfirmedExecution()
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

        await _handler.HandleAsync(order.Id, request);

        _doerlyRequestContextMock.SetupProperty(x => x.UserId, order.ExecutorId);

        request = new UpdateOrderStatusRequest
        {
            Status = EOrderStatus.Completed
        };

        var result = await _handler.HandleAsync(order.Id, request);

        order = await OrderDbContext.Orders.FindAsync(order.Id);

        Assert.Equal(EOrderStatus.Completed, order.Status);
        Assert.NotNull(order.ExecutionDate);
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
