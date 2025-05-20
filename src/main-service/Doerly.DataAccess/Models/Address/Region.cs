using System.ComponentModel.DataAnnotations.Schema;
using Doerly.DataAccess.Constants;

namespace Doerly.DataAccess.Models;

[Table(AddressDbConstants.Tables.Region, Schema = AddressDbConstants.AddressSchema)]
public class Region : BaseEntity
{
    public string Name { get; set; }
}