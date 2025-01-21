using System.Linq.Expressions;

namespace HR4You.Model.Base.Pagination;

public static class CustomExpressionFilter<T> where T : class
{
    public static Expression<Func<T, bool>> CreateFilter(List<ColumnFilter> columnFilters)
    {
        Expression<Func<T, bool>> filters;
        try
        {
            // Create the parameter expression for the input data
            var parameter = Expression.Parameter(typeof(T));

            // Build the filter expression dynamically
            Expression filterExpression = null;
            foreach (var filter in columnFilters)
            {
                var property = Expression.Property(parameter, filter.ColumnName);

                // Determine datatype and comparision operator used for filtering
                Expression comparisonOperation;
                if (property.Type == typeof(string))
                {
                    var constant = Expression.Constant(filter.Value);
                    comparisonOperation = Expression.Call(property, "Contains", Type.EmptyTypes, constant);
                }
                else if (property.Type == typeof(double))
                {
                    var constant = Expression.Constant(Convert.ToDouble(filter.Value));
                    comparisonOperation = Expression.Equal(property, constant);
                }
                else if (property.Type == typeof(Guid))
                {
                    var constant = Expression.Constant(Guid.Parse(filter.Value));
                    comparisonOperation = Expression.Equal(property, constant);
                }
                else if (property.Type == typeof(float))
                {
                    var constant = Expression.Constant(float.Parse(filter.Value));
                    comparisonOperation = Expression.Equal(property, constant);
                }
                else
                {
                    var constant = Expression.Constant(Convert.ToInt32(filter.Value));
                    comparisonOperation = Expression.Equal(property, constant);
                }

                filterExpression = filterExpression switch
                {
                    null => comparisonOperation,
                    _ => Expression.And(filterExpression, comparisonOperation)
                };
            }

            // Create the lambda expression with the parameter and the filter expression
            filters = Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
        }
        catch (Exception)
        {
            filters = null;
        }
        return filters;
    }

}