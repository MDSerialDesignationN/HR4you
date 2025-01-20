using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HR4You.Model.Base;

//https://learn.microsoft.com/en-us/aspnet/core/mvc/models/model-binding?view=aspnetcore-9.0
public class MetadataValueModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        ArgumentNullException.ThrowIfNull(bindingContext);

        var values = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

        if (values.Length == 0)
            return Task.CompletedTask;
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var deserialized = JsonSerializer.Deserialize(values.FirstValue!, bindingContext.ModelType, options);

        bindingContext.Result = ModelBindingResult.Success(deserialized);
        return Task.CompletedTask;
    }
}