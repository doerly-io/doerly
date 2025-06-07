using Doerly.DataTransferObjects.Pagination;

namespace Doerly.Module.Profile.Contracts.Dtos;

public class ProfileQueryDto : PageInfo
{
    public string? Name { get; set; }
}
