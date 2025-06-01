using Doerly.DataAccess.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Catalog.DataAccess.Models
{
    public class Service : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public int UserId { get; set; }

        public decimal? Price { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<ServiceFilterValue> FilterValues { get; set; } = new List<ServiceFilterValue>();
    }
}
