using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Catalog.Contracts.Responses;
using Doerly.Module.Catalog.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Catalog.Domain.Handlers.Service
{
    public class GetServiceByIdHandler : BaseCatalogHandler
    {
        public GetServiceByIdHandler(CatalogDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<OperationResult<GetServiceResponse>> HandleAsync(int id)
        {
            var serviceDto = await DbContext.Services
                .Where(s => s.Id == id)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.Description,
                    s.CategoryId,
                    CategoryName = s.Category.Name,
                    s.UserId,
                    s.Price,
                    s.IsDeleted,
                    s.IsEnabled
                })
                .FirstOrDefaultAsync();

            if (serviceDto == null)
                return OperationResult.Failure<GetServiceResponse>(Resources.Get("ServiceNotFound"));

            return OperationResult.Success(new GetServiceResponse
            {
                Id = serviceDto.Id,
                Name = serviceDto.Name,
                Description = serviceDto.Description,
                CategoryId = serviceDto.CategoryId,
                CategoryName = serviceDto.CategoryName,
                UserId = serviceDto.UserId,
                Price = serviceDto.Price,
                IsDeleted = serviceDto.IsDeleted,
                IsEnabled = serviceDto.IsEnabled
            });
        }

    }
}
