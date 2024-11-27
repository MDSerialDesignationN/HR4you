using HR4You.Context.HourEntry;
using Microsoft.EntityFrameworkCore;

namespace HR4You.Context;

public class ModelContextHelper
{
    public static void ConfigureModelContexts(WebApplicationBuilder webApplicationBuilder, string? connectionString)
    {
        webApplicationBuilder.Services.AddHttpContextAccessor();
        
        //HourEntryContext
        webApplicationBuilder.Services.AddScoped(sp =>
        {
            var optionsBuilder = new DbContextOptionsBuilder<ModelBaseContext<Model.Base.Models.HourEntry>>();
            optionsBuilder.UseMySql(connectionString!, ServerVersion.AutoDetect(connectionString));
            
            return new HourEntryContext(optionsBuilder.Options, sp.GetService<ILogger<HourEntryContext>>()!, sp);
        });

        
        //Register remaining handler
        webApplicationBuilder.Services.AddSingleton<ModelChecker>();

    }

    public static void MigrateModelDb(WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        
        var hc = scope.ServiceProvider.GetService<HourEntryContext>()!;
        hc.Database.Migrate();
    }
}