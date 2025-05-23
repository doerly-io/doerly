using Doerly.DataAccess.Models;

namespace Doerly.Module.Common.DataAccess.Address.Entities;

public class City : BaseEntity
{
    public string Name { get; set; }
    
    public int RegionId { get; set; }
    
    public virtual Region Region { get; set; }
}
