namespace Doerly.DataTransferObjects.Pagination;

public class PageDto<T>
{
    public int PageSize { get; set; }
    public int TotalSize { get; set; }
    public int PagesCount { get; set; }
    public List<T> List { get; set; }
}