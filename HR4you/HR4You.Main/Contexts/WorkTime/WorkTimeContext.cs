using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HR4You.Contexts.WorkTime
{
    public class WorkTimeContext : ModelBaseContext<Model.Base.Models.WorkTime.WorkTime>
    {
        public WorkTimeContext(DbContextOptions<ModelBaseContext<Model.Base.Models.WorkTime.WorkTime>> options,
            ILogger<WorkTimeContext> logger, IServiceProvider serviceProvider) : base(options, logger)
        {
        }
        
        public async Task<Model.Base.Models.WorkTime.WorkTime?> GetActiveConfig()
        {
            var entity = await Entities.FirstOrDefaultAsync(wt => wt.Deleted == false);
            return entity;
        }

        public class WorkTimeContextDesignTimeFactory : IDesignTimeDbContextFactory<WorkTimeContext>
        {
            public WorkTimeContext CreateDbContext(string[] args)
            {
                var builder = new DbContextOptionsBuilder<ModelBaseContext<Model.Base.Models.WorkTime.WorkTime>>();

                builder.UseMySql(args[0], ServerVersion.AutoDetect(args[0]));
                return new WorkTimeContext(builder.Options, null!, null!);
            }
        }
    }
}