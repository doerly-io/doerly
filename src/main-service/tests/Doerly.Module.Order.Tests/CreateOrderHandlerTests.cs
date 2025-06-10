using Doerly.Domain;
using Doerly.Domain.Exceptions;
using Doerly.Domain.Models;
using Doerly.FileRepository;
using Doerly.Messaging;
using Doerly.Module.Order.DataAccess.Entities;
using Doerly.Module.Order.DataTransferObjects.Requests;
using Doerly.Module.Order.Domain.Handlers;
using Doerly.Module.Order.Enums;
using Doerly.Module.Profile.DataTransferObjects;
using Doerly.Proxy.Profile;

using Microsoft.AspNetCore.Http;

using Moq;

namespace Doerly.Module.Order.Tests;
public class CreateOrderHandlerTests : BaseOrderTests
{
    public CreateOrderHandler _handler;

    private readonly Mock<IDoerlyRequestContext> _doerlyRequestContextMock;
    private readonly Mock<IProfileModuleProxy> _profileModuleProxyMock;
    private readonly Mock<IFileRepository> _fileRepositoryMock;
    private readonly Mock<IMessagePublisher> _messagePublisherMock;

    public CreateOrderHandlerTests(PostgresTestContainerFixture fixture) : base(fixture)
    {
        _doerlyRequestContextMock = new Mock<IDoerlyRequestContext>();
        _profileModuleProxyMock = new Mock<IProfileModuleProxy>();
        _fileRepositoryMock = new Mock<IFileRepository>();
        _messagePublisherMock = new Mock<IMessagePublisher>();

        _handler = new CreateOrderHandler(OrderDbContext, _doerlyRequestContextMock.Object, _profileModuleProxyMock.Object,
            _fileRepositoryMock.Object, _messagePublisherMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ShouldCreateOrder_WhenRequestIsValid()
    {
        // Arrange
        var userId = 123;
        _doerlyRequestContextMock.SetupProperty(x => x.UserId, userId);

        var request = new CreateOrderRequest
        {
            CategoryId = 1,
            Name = "Test Order",
            Description = "This is a test order description.",
            Price = 100.50m,
            PaymentKind = EPaymentKind.Online,
            DueDate = DateTime.UtcNow.AddDays(7),
            IsPriceNegotiable = false,
            UseProfileAddress = false,
            RegionId = 1,
            CityId = 1
        };

        var files = new List<IFormFile>();

        // Act
        var result = await _handler.HandleAsync(request, files);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.True(result.Value.Id > 0);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowException_WhenAddressIsEmpty()
    {
        // Arrange
        var userId = 123;
        _doerlyRequestContextMock.SetupProperty(x => x.UserId, userId);

        var request = new CreateOrderRequest
        {
            CategoryId = 1,
            Name = "Test Order",
            Description = "This is a test order description.",
            Price = 100.50m,
            PaymentKind = EPaymentKind.Online,
            DueDate = DateTime.UtcNow.AddDays(7),
            IsPriceNegotiable = false,
            UseProfileAddress = true
        };

        var files = new List<IFormFile>();

        // Act & Assert
        await Assert.ThrowsAsync<DoerlyException>(() => _handler.HandleAsync(request, files));
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnFailure_WhenUserIsUnauthorized()
    {
        // Arrange
        var request = new CreateOrderRequest
        {
            CategoryId = 1,
            Name = "Test Order",
            Description = "This is a test order description.",
            Price = 100.50m,
            PaymentKind = EPaymentKind.Online,
            DueDate = DateTime.UtcNow.AddDays(7),
            IsPriceNegotiable = false
        };

        var files = new List<IFormFile>();

        // Act & Assert
        await Assert.ThrowsAsync<DoerlyException>(() => _handler.HandleAsync(request, files));
    }
}
