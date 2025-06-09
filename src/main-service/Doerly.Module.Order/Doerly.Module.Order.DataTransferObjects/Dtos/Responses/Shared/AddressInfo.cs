using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Order.DataTransferObjects.Dtos;
public class AddressInfo
{
    public int RegionId { get; set; }

    public string RegionName { get; set; }

    public int CityId { get; set; }

    public string CityName { get; set; }
}
