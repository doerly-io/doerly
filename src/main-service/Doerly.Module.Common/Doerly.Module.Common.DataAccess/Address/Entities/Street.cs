using Doerly.DataAccess.Models;

namespace Doerly.Module.Common.DataAccess.Address.Entities;

public class Street : BaseEntity
{
    public string Name { get; set; }
    
    public string ZipCode { get; set; }
    
    public int RegionId { get; set; }
    public virtual Region Region { get; set; }
    
    public int CityId { get; set; }
    public virtual City City { get; set; }
}
