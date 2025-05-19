using System.ComponentModel.DataAnnotations.Schema;
using Doerly.DataAccess.Constants;

namespace Doerly.DataAccess.Models;

[Table(AddressDbConstants.Tables.Street, Schema = AddressDbConstants.AddressSchema)]
public class Street : BaseEntity
{
    public string Name { get; set; }
    
    public string ZipCode { get; set; }
    
    public int RegionId { get; set; }
    public virtual Region Region { get; set; }
    
    public int CityId { get; set; }
    public virtual City City { get; set; }
}