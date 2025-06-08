using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doerly.Module.Order.Domain.Handlers.Order;
using Doerly.Module.Order.Contracts.Dtos;
using Moq;
using Doerly.Module.Order.Domain.Handlers;
using Doerly.Domain;
using Doerly.FileRepository;
using Doerly.Proxy.Profile;

namespace Doerly.Module.Order.Tests;
public class UpdateOrderHandlerTests : BaseOrderTests
{
    private readonly UpdateOrderHandler _handler;
    private readonly Mock<IDoerlyRequestContext> _doerlyRequestContextMock;

    public UpdateOrderHandlerTests(PostgresTestContainerFixture fixture) : base(fixture)
    {
        _doerlyRequestContextMock = new Mock<IDoerlyRequestContext>();
        var profileModuleProxyMock = new Mock<IProfileModuleProxy>();
        var fileRepositoryMock = new Mock<IFileRepository>();

        _handler = new UpdateOrderHandler(OrderDbContext, _doerlyRequestContextMock.Object, fileRepositoryMock.Object, profileModuleProxyMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ShouldUpdateOrder_WhenRequestIsValid()
    {
        var order = await AddTestOrderAsync();

        _doerlyRequestContextMock.SetupProperty(x => x.UserId, order.CustomerId);   

        var request = new UpdateOrderRequest
        {
            Name = "Updated Name",
            Description = "Updated Description",
            Price = order.Price + 10,
            //ServiceId = order.ServiceId,
            PaymentKind = order.PaymentKind,
            DueDate = order.DueDate.AddDays(1),
            IsPriceNegotiable = !order.IsPriceNegotiable
        };

        var result = await _handler.HandleAsync(order.Id, request, [], []);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnFailure_WhenOrderDoesNotExist()
    {
        var request = new UpdateOrderRequest
        {
            Name = "Name",
            Description = "Desc"
        };
        var result = await _handler.HandleAsync(-1, request, [], []);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnFailure_WhenUserIsUnauthorized()
    {
        var order = await AddTestOrderAsync();

        var request = new UpdateOrderRequest
        {
            Name = "Updated Name",
            Description = "Updated Description",
            Price = order.Price + 10,
            //ServiceId = order.ServiceId,
            PaymentKind = order.PaymentKind,
            DueDate = order.DueDate.AddDays(1),
            IsPriceNegotiable = !order.IsPriceNegotiable
        };

        var result = await _handler.HandleAsync(order.Id, request, [], []);
        Assert.False(result.IsSuccess);
    }
}
