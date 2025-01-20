using Microsoft.AspNetCore.Mvc;

namespace HR4You.Model.Base.Pagination;

[ModelBinder(BinderType = typeof(MetadataValueModelBinder))]
public class ColumnFilter
{
    public required string ColumnName { get; set; }
    public required string Value { get; set; }
}