using Doerly.DataAccess.Models;
using Doerly.Module.Catalog.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Catalog.DataAccess.Models
{
    public class Filter : BaseEntity
    {
        public string Name { get; set; } = null!;
        public int Type { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public ICollection<ServiceFilterValue> FilterValues { get; set; } = new List<ServiceFilterValue>();
    }
}
