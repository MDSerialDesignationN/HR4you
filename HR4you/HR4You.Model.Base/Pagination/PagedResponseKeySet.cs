namespace HR4You.Model.Base.Pagination;

public record PagedResponseKeySet<T>
{
    public int Reference { get; init; }
    public List<T> Data { get; init; }

    public PagedResponseKeySet(List<T> data, int reference)
    {
        Data = data;
        Reference = reference;
    }
}