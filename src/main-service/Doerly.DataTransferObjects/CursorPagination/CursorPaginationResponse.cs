namespace Doerly.DataTransferObjects;

public class CursorPaginationResponse<T>
{
    public IEnumerable<T> Items { get; set; }

    public string? Cursor { get; set; }
}
