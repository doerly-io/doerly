using Doerly.DataTransferObjects.Pagination;

namespace Doerly.Module.Profile.Contracts.Dtos;

public class LanguagesQueryDto : PageInfo
{
    public string? Name { get; set; }
}