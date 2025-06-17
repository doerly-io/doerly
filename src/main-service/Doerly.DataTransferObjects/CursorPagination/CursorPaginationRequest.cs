namespace Doerly.DataTransferObjects;

public class CursorPaginationRequest
{
    public int PageSize { get; set; } = 10;

    public string? Cursor { get; set; }
}
