namespace HR4You.Model.Base.Pagination;


public record PagedResponseOffset<T>
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
    public int TotalRecords { get; init; }
    public List<T> Data { get; init; }

    public PagedResponseOffset(int pageNumber, int pageSize, int totalRecords, List<T> data)
    {
        Data = data;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        TotalPages = (int)Math.Ceiling((decimal)totalRecords / pageSize);
    }
}