using Doerly.Domain;
using Doerly.Domain.Exceptions;
using Doerly.FileRepository;
using Doerly.Module.Order.DataAccess.Entities;
using Doerly.Module.Order.DataTransferObjects.Requests;
using Doerly.Module.Order.Domain.Handlers;
using Doerly.Proxy.Profile;

using Moq;

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
            PaymentKind = order.PaymentKind,
            DueDate = order.DueDate.AddDays(1),
            IsPriceNegotiable = !order.IsPriceNegotiable,
            UseProfileAddress = false,
            RegionId = order.RegionId,
            CityId = order.CityId
        };

        var result = await _handler.HandleAsync(order.Id, request, [], []);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnFailure_WhenOrderDoesNotExist()
    {
        _doerlyRequestContextMock.SetupProperty(x => x.UserId, 1);

        var request = new UpdateOrderRequest
        {
            Name = "Name",
            Description = "Desc"
        };
        await Assert.ThrowsAsync<DoerlyException>(async() => await _handler.HandleAsync(-1, request, [], []));
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
            PaymentKind = order.PaymentKind,
            DueDate = order.DueDate.AddDays(1),
            IsPriceNegotiable = !order.IsPriceNegotiable
        };

        await Assert.ThrowsAsync<DoerlyException>(async () => await _handler.HandleAsync(order.Id, request, [], []));
    }
}
