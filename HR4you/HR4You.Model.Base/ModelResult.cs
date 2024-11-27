namespace HR4You.Model.Base;

public class ModelResult<T>
{
    public T? Entity { get; set; }
    public MasterDataError Error { get; set; }

    public static ModelResult<T> NotFound()
    {
        return new ModelResult<T>
        {
            Entity = default,
            Error = MasterDataError.NotFound
        };
    }
    
    public static ModelResult<T> Ok(T e)
    {
        return new ModelResult<T>
        {
            Entity = e,
            Error = MasterDataError.None
        };
    }
}

public enum MasterDataError
{
    None,
    NotFound
}