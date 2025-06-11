using Doerly.Domain.Models;
using Doerly.FileRepository;
using Doerly.Module.Common.DataAccess.Address;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.Domain.Handlers;
using Doerly.Module.Profile.DataTransferObjects;
using Doerly.Proxy.Profile;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
        // Create a configuration for the in-memory database
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:AddressConnection"] = fixture.ConnectionString
            })
            .Build();

        var addressDbContext = new AddressDbContext(configuration);
        addressDbContext.Database.Migrate();

        _handler = new GetOrderByIdHandler(OrderDbContext, _profileModuleProxyMock.Object, addressDbContext, _fileRepositoryMock.Object);
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
