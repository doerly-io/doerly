namespace Doerly.Module.Profile.DataTransferObjects;

public class ProfileAddressDto
{
    public required int CityId { get; set; }
    public required string CityName { get; set; }
    public required int RegionId { get; set; }
    public required string RegionName { get; set; }
} 
