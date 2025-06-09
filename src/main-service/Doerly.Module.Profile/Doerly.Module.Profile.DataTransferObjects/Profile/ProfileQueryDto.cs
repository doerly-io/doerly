using Doerly.DataTransferObjects.Pagination;

namespace Doerly.Module.Profile.DataTransferObjects;

public class ProfileQueryDto : PageInfo
{
    public string? Name { get; set; }
}
