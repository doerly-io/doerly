using Doerly.DataAccess.Enums.Address;

namespace Doerly.Module.Profile.Contracts.Dtos;

public class AddressDto
{
    public required int StreetId { get; set; }
    public required string StreetName { get; set; }
    public required int CityId { get; set; }
    public required string CityName { get; set; }
    public required int RegionId { get; set; }
    public required string RegionName { get; set; }
    public required Country Country { get; set; }
    public required string PostIndex { get; set; }
} 