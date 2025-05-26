using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Catalog.DataAccess;
using Doerly.Module.Catalog.Domain.Dtos.Responses.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Catalog.Domain.Handlers.Category
{
    public class GetCategoryByIdHandler : BaseCatalogHandler
    {
        public GetCategoryByIdHandler(CatalogDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<HandlerResult<GetCategoryResponse>> HandleAsync(int id)
        {
            var category = await DbContext.Categories.FindAsync(id);
            if (category == null)
                return HandlerResult.Failure<GetCategoryResponse>(Resources.Get("CATEGORY_NOT_FOUND"));

            var categoryDto = new GetCategoryResponse
            {
                Id = order.Id,
                CategoryId = order.CategoryId,
                Name = order.Name,
                Description = order.Description,
                Price = order.Price,
                PaymentKind = order.PaymentKind,
                DueDate = order.DueDate,
                Status = order.Status,
                CustomerId = order.CustomerId,
                ExecutorId = order.ExecutorId,
                ExecutionDate = order.ExecutionDate,
                BillId = order.BillId
            };

            return HandlerResult.Success(categoryDto);
        }
    }
}
