using Doerly.DataTransferObjects.Pagination;

namespace Doerly.Module.Profile.DataTransferObjects;

public class LanguagesQueryDto : PageInfo
{
    public string? Name { get; set; }
}