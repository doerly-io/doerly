namespace Doerly.DataTransferObjects.Pagination;

public class BasePaginationResponse<T>
{
    public int Count { get; set; }

    public IEnumerable<T> Items { get; set; }
}