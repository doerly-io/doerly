using System.Collections.Generic;
using System.Threading.Tasks;
using Doerly.Domain.Models;
using Doerly.Module.Catalog.Contracts.Requests;
using Doerly.Module.Catalog.DataAccess.Models;
using Doerly.Module.Catalog.Domain.Handlers.Service;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Doerly.Module.Catalog.Tests
{
    public class CreateServiceHandlerTests : BaseCatalogTests
    {
        private readonly CreateServiceHandler _handler;

        public CreateServiceHandlerTests(PostgresTestContainerFixture fixture) : base(fixture)
        {
            _handler = new CreateServiceHandler(CatalogDbContext);
        }

        [Fact]
        public async Task HandleAsync_ShouldCreateService_WhenRequestIsValid()
        {
            // Arrange
            var category = new Category { Name = "Test Category", IsDeleted = false };
            await CatalogDbContext.Categories.AddAsync(category);
            await CatalogDbContext.SaveChangesAsync();

            var filter = new Filter
            {
                Name = "Test Filter",
                CategoryId = category.Id
            };
            await CatalogDbContext.Filters.AddAsync(filter);
            await CatalogDbContext.SaveChangesAsync();

            var request = new CreateServiceRequest
            {
                Name = "Test Service",
                Description = "Test Description",
                Price = 99.99m,
                UserId = 1,
                CategoryId = category.Id,
                FilterValues = new List<FilterValueRequest>
                {
                    new FilterValueRequest
                    {
                        FilterId = filter.Id,
                        Value = "Filter Value"
                    }
                }
            };

            // Act
            var result = await _handler.HandleAsync(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value > 0);

            var service = await CatalogDbContext.Services
                .Include(s => s.FilterValues)
                .FirstOrDefaultAsync(s => s.Id == result.Value);

            Assert.NotNull(service);
            Assert.Equal(request.Name, service.Name);
            Assert.Equal(request.Description, service.Description);
            Assert.Single(service.FilterValues);
            Assert.Equal("Filter Value", service.FilterValues.First().Value);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailure_WhenCategoryDoesNotExist()
        {
            // Arrange
            var request = new CreateServiceRequest
            {
                Name = "Invalid Service",
                Description = "Invalid",
                Price = 10m,
                UserId = 1,
                CategoryId = -1, // Неіснуюча категорія
                FilterValues = new List<FilterValueRequest>()
            };

            // Act
            var result = await _handler.HandleAsync(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Category not found", result.ErrorMessage);
        }
    }
}
