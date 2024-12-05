using HR4You.Contexts.Customer;
using HR4You.Contexts.HourEntry;
using HR4You.Contexts.Project;
using HR4You.Contexts.Tag;
using HR4You.Contexts.WorkTime;
using Microsoft.EntityFrameworkCore;

namespace HR4You.Contexts;

public static class ModelContextHelper
{
    public static void ConfigureModelContexts(WebApplicationBuilder webApplicationBuilder, string? connectionString)
    {
        webApplicationBuilder.Services.AddHttpContextAccessor();
        
        //HourEntryContext
        webApplicationBuilder.Services.AddScoped(sp =>
        {
            var optionsBuilder = new DbContextOptionsBuilder<ModelBaseContext<Model.Base.Models.HourEntry.HourEntry>>();
            optionsBuilder.UseMySql(connectionString!, ServerVersion.AutoDetect(connectionString));
            
            return new HourEntryContext(optionsBuilder.Options, sp.GetService<ILogger<HourEntryContext>>()!, sp);
        });
        //WorkTimeContext
        webApplicationBuilder.Services.AddScoped(sp =>
        {
            var optionsBuilder = new DbContextOptionsBuilder<ModelBaseContext<Model.Base.Models.WorkTime.WorkTime>>();
            optionsBuilder.UseMySql(connectionString!, ServerVersion.AutoDetect(connectionString));
            
            return new WorkTimeContext(optionsBuilder.Options, sp.GetService<ILogger<WorkTimeContext>>()!, sp);
        });
        //CustomerContext
        webApplicationBuilder.Services.AddScoped(sp =>
        {
            var optionsBuilder = new DbContextOptionsBuilder<ModelBaseContext<Model.Base.Models.Customer.Customer>>();
            optionsBuilder.UseMySql(connectionString!, ServerVersion.AutoDetect(connectionString));
            
            return new CustomerContext(optionsBuilder.Options, sp.GetService<ILogger<CustomerContext>>()!, sp);
        });
        //ProjectContext
        webApplicationBuilder.Services.AddScoped(sp =>
        {
            var optionsBuilder = new DbContextOptionsBuilder<ModelBaseContext<Model.Base.Models.Project.Project>>();
            optionsBuilder.UseMySql(connectionString!, ServerVersion.AutoDetect(connectionString));
            
            return new ProjectContext(optionsBuilder.Options, sp.GetService<ILogger<ProjectContext>>()!, sp);
        });
        //TagContext
        webApplicationBuilder.Services.AddScoped(sp =>
        {
            var optionsBuilder = new DbContextOptionsBuilder<ModelBaseContext<Model.Base.Models.Tag.Tag>>();
            optionsBuilder.UseMySql(connectionString!, ServerVersion.AutoDetect(connectionString));
            
            return new TagContext(optionsBuilder.Options, sp.GetService<ILogger<TagContext>>()!, sp);
        });
        
        //Register remaining handler
        webApplicationBuilder.Services.AddSingleton<ModelChecker>();

    }

    public static void MigrateModelDb(WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        
        //Migrates HourEntry and all dependent tables!
        var hc = scope.ServiceProvider.GetService<HourEntryContext>()!;
        hc.Database.Migrate();
        
        var wt = scope.ServiceProvider.GetService<WorkTimeContext>()!;
        wt.Database.Migrate();
        
        var f = scope.ServiceProvider.GetService<TagContext>()!;
        f.Database.Migrate();
    }
}