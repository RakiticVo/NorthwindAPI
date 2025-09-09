namespace NorthwindApi.Application.DTOs;

public record PagedResponse<T>
{
    public List<T> Items { get; set; } = [];
    public int PageNumber { get; set; }
    private int PageSize { get; set; }
    private int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    public PagedResponse(List<T> items, int pageNumber, int pageSize, int totalCount)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
    }
}
