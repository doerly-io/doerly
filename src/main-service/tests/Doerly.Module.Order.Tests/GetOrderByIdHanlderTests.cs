using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Doerly.Domain.Models;
using Doerly.FileRepository;
using Doerly.Module.Common.DataAccess.Address;
using Doerly.Module.Order.Contracts.Dtos;
using Doerly.Module.Order.Domain.Handlers;
using Doerly.Module.Order.Domain.Handlers.Order;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Proxy.Payment;
using Doerly.Proxy.Profile;

using Moq;

namespace Doerly.Module.Order.Tests;
public class GetOrderByIdHandlerTests : BaseOrderTests
{
    private readonly GetOrderByIdHandler _handler;
    private readonly Mock<IFileRepository> _fileRepositoryMock;
    private readonly Mock<IProfileModuleProxy> _profileModuleProxyMock;

    public GetOrderByIdHandlerTests(PostgresTestContainerFixture fixture) : base(fixture)
    {
        _fileRepositoryMock = new Mock<IFileRepository>();
        _profileModuleProxyMock = new Mock<IProfileModuleProxy>();
        var addressDbContext = new Mock<AddressDbContext>();

        _handler = new GetOrderByIdHandler(OrderDbContext, _profileModuleProxyMock.Object, addressDbContext.Object, _fileRepositoryMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnOrder_WhenOrderExists()
    {
        // Arrange
        var order = await AddTestOrderAsync();

        var profileDto = new ProfileDto
        {
            Id = order.CustomerId,
            FirstName = "Test",
            LastName = "User",
            ImageUrl = null,
        };

        _profileModuleProxyMock
            .Setup(x => x.GetProfileAsync(order.CustomerId))
            .ReturnsAsync(HandlerResult.Success(profileDto));

        // Act
        var result = await _handler.HandleAsync(order.Id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(order.Id, result.Value.Id);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnFailure_WhenOrderDoesNotExist()
    {
        // Act
        var result = await _handler.HandleAsync(-1);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Value);
    }
}
