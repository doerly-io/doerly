using Doerly.DataTransferObjects.Pagination;
using Doerly.Domain.Models;
using Doerly.Module.Order.DataTransferObjects.Requests;
using Doerly.Module.Order.Domain.Handlers;
using Doerly.Module.Profile.DataTransferObjects;
using Doerly.Proxy.Profile;

using Moq;

namespace Doerly.Module.Order.Tests;
public class GetOrdersWithPaginationHandlerTests : BaseOrderTests
{
    private readonly GetOrdersWithPaginationHandler _handler;
    private readonly Mock<IProfileModuleProxy> _profileModuleProxyMock;

    public GetOrdersWithPaginationHandlerTests(PostgresTestContainerFixture fixture) : base(fixture)
    {
        _profileModuleProxyMock = new Mock<IProfileModuleProxy>();

        _handler = new GetOrdersWithPaginationHandler(OrderDbContext, _profileModuleProxyMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnPagedOrders()
    {
        var order1 = await AddTestOrderAsync();
        var order2 = await AddTestOrderAsync();

        var profileDtos = new List<ProfileDto>
        {
            new ProfileDto
            {
                Id = order1.CustomerId,
                FirstName = "Test",
                LastName = "User",
                ImageUrl = null,
            },
        };

        _profileModuleProxyMock
            .Setup(x => x.GetProfilesAsync(It.IsAny<int[]>()))
            .ReturnsAsync(OperationResult.Success((IEnumerable<ProfileDto>)profileDtos));

        var request = new GetOrdersWithPaginationRequest
        {
            PageInfo = new PageInfo
            {
                Number = 1,
                Size = 10
            }
        };

        var result = await _handler.HandleAsync(request);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.True(result.Value.Orders.Count == 2);
    }
}
