using Doerly.Domain.Models;
using Doerly.Module.Catalog.Contracts.Dtos.Requests.Category;
using Doerly.Module.Catalog.Contracts.Dtos.Responses.Category;
using Doerly.Module.Catalog.DataAccess;
using CategoryEntity = Doerly.Module.Catalog.DataAccess.Models.Category;

namespace Doerly.Module.Catalog.Domain.Handlers.Category
{
    public class CreateCategoryHandler : BaseCatalogHandler
    {
        public CreateCategoryHandler(CatalogDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<HandlerResult<CreateCategoryResponse>> HandleAsync(CreateCategoryRequest dto)
        {
            var category = new CategoryEntity()
            {
                Name = dto.Name,
                Description = dto.Description,
                IsDeleted = false,
                IsEnabled = dto.IsEnabled,
            };

            DbContext.Categories.Add(category);
            await DbContext.SaveChangesAsync();

            var result = new CreateCategoryResponse()
            {
                Id = category.Id
            };

            return HandlerResult.Success(result);
        }
    }
}
