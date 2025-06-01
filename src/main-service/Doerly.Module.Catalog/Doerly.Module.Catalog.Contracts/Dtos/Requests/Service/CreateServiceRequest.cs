using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Catalog.Contracts.Dtos.Requests.Service
{
    public class CreateServiceRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public decimal? Price { get; set; }

        public Dictionary<int, string> FilterValues { get; set; } = new();
    }
}
