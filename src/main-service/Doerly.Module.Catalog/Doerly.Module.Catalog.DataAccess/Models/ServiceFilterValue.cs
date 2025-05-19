using Doerly.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Catalog.DataAccess.Models
{
    public class ServiceFilterValue : BaseEntity
    {
        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;

        public int FilterId { get; set; }
        public Filter Filter { get; set; } = null!;

        public string Value { get; set; } = null!;
    }
}
