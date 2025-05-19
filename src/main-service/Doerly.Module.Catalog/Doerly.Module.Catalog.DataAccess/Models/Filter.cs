using Doerly.DataAccess.Models;
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
        public string Type { get; set; } = null!;
        public List<string>? Options { get; set; }

        public bool IsCustomInput { get; set; }

        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;
    }
}
