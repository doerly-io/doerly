using Doerly.Module.Catalog.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Catalog.Contracts.Dtos.Responses.Filter
{
    public class GetFilterResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public EFilterType Type { get; set; }
        public List<string>? Options { get; set; }
        public int CategoryId { get; set; }
    }
}
