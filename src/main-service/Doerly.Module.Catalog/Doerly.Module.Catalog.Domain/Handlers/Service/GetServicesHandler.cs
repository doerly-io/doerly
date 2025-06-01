using Doerly.Domain.Models;
using Doerly.Module.Catalog.Contracts.Dtos.Responses.Service;
using Doerly.Module.Catalog.DataAccess;
using Microsoft.EntityFrameworkCore;
using CategoryEntity = Doerly.Module.Catalog.DataAccess.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Catalog.Domain.Handlers.Service
{
    public class GetServicesHandler : BaseCatalogHandler
    {
        public GetServicesHandler(CatalogDbContext dbContext) : base(dbContext) 
        { 
        }

        public async Task<HandlerResult<List<GetServiceResponse>>> HandleAsync()
        {
            var services = await DbContext.Services
                .AsNoTracking()
                .Where(s => !s.IsDeleted)
                .Include(s => s.Category)!
                    .ThenInclude(c => c.Parent)!
                        .ThenInclude(p => p.Parent)
                .ToListAsync();

            var result = services.Select(s => new GetServiceResponse
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Price = s.Price,
                UserId = s.UserId,
                IsEnabled = s.IsEnabled,
                IsDeleted = s.IsDeleted,
                CategoryId = s.CategoryId,
                CategoryName = s.Category.Name,
                CategoryPath = GetCategoryPath(s.Category)
            }).ToList();

            return HandlerResult.Success(result);
        }

        private List<string> GetCategoryPath(CategoryEntity? category)
        {
            var path = new List<string>();
            while (category != null)
            {
                path.Insert(0, category.Name);
                category = category.Parent;
            }
            return path;
        }
    }

}
