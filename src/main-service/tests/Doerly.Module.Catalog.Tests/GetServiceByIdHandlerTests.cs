using Doerly.Domain;
using Doerly.Domain.Models;
using Doerly.Module.Catalog.Contracts.Responses;
using Doerly.Module.Catalog.DataAccess;
using Doerly.Module.Catalog.DataAccess.Models;
using Doerly.Module.Catalog.Domain.Handlers.Service;
using Doerly.Module.Catalog.Enums;
using Doerly.Module.Profile.DataTransferObjects;
using Doerly.Proxy.Profile;

using Microsoft.EntityFrameworkCore;

using Moq;
using Xunit;

namespace Doerly.Module.Catalog.Tests
{
    public class GetServiceByIdHandlerTests : BaseCatalogTests
    {
        private readonly GetServiceByIdHandler _handler;
        private readonly Mock<IProfileModuleProxy> _profileModuleProxyMock;

        public GetServiceByIdHandlerTests(PostgresTestContainerFixture fixture) : base(fixture)
        {
            _profileModuleProxyMock = new Mock<IProfileModuleProxy>();

            _handler = new GetServiceByIdHandler(CatalogDbContext, _profileModuleProxyMock.Object);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailure_WhenServiceDoesNotExist()
        {
            // Act
            var result = await _handler.HandleAsync(-1);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnSuccess_WhenServiceExists()
        {
            // Arrange
            var category = new Category { Name = "Test Category" };
            await CatalogDbContext.Categories.AddAsync(category);
            await CatalogDbContext.SaveChangesAsync();

            var filter = new Filter
            {
                Name = "Color",
                Type = EFilterType.Checkbox,
                CategoryId = category.Id
            };
            await CatalogDbContext.Filters.AddAsync(filter);
            await CatalogDbContext.SaveChangesAsync();

            var service = new Service
            {
                Name = "Test Service",
                Description = "Test Desc",
                CategoryId = category.Id,
                UserId = 100,
                Price = 50,
                IsDeleted = false,
                IsEnabled = true,
                FilterValues = new List<ServiceFilterValue>
        {
            new ServiceFilterValue
            {
                FilterId = filter.Id,
                Value = "Red"
            }
        }
            };

            await CatalogDbContext.Services.AddAsync(service);
            await CatalogDbContext.SaveChangesAsync();

            _profileModuleProxyMock
                .Setup(x => x.GetProfileAsync(service.UserId))
                .ReturnsAsync(OperationResult.Success(new ProfileDto
                {
                    Id = service.UserId,
                    FirstName = "Test User",
                    LastName = "Last Name"
                }));

            // Act
            var result = await _handler.HandleAsync(service.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("Test Service", result.Value.Name);
            Assert.Equal("Test User", result.Value.User.FirstName);
            Assert.Single(result.Value.FilterValues);
            Assert.Equal("Red", result.Value.FilterValues[0].Value);
        }
    }
}
