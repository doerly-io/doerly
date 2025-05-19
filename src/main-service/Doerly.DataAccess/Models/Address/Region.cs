using System.ComponentModel.DataAnnotations.Schema;
using Doerly.DataAccess.Constants;
using Doerly.DataAccess.Enums.Address;

namespace Doerly.DataAccess.Models;

[Table(AddressDbConstants.Tables.Region, Schema = AddressDbConstants.AddressSchema)]
public class Region : BaseEntity
{
    public string Name { get; set; }
    
    public Country Country { get; set; }
}