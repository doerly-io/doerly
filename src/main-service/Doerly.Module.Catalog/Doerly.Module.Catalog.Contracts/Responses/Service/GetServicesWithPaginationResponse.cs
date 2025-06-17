using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Catalog.Contracts.Responses
{
    public class GetServicesWithPaginationResponse
    {
        public int Total { get; set; }

        public List<GetServiceResponse> Services { get; set; } = new List<GetServiceResponse>();
    }
}
