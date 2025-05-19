using System.ComponentModel.DataAnnotations.Schema;
using Doerly.DataAccess.Constants;

namespace Doerly.DataAccess.Models;

[Table(AddressDbConstants.Tables.City, Schema = AddressDbConstants.AddressSchema)]
public class City : BaseEntity
{
    public string Name { get; set; }
    
    public int RegionId { get; set; }
    public virtual Region Region { get; set; }
}