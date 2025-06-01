using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Catalog.Contracts.Dtos.Responses.Category
{
    public class GetCategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsEnabled { get; set; }
        public List<GetCategoryResponse> Children { get; set; } = new();
    }
}
