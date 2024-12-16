using HR4You.Model.Base.Models.WorkTime;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;

namespace HR4You.Contexts.WorkTime
{
    public class WorkTimeContext : ModelBaseContext<Model.Base.Models.WorkTime.WorkTime>
    {
        private readonly IServiceProvider _serviceProvider;

        public WorkTimeContext(DbContextOptions<ModelBaseContext<Model.Base.Models.WorkTime.WorkTime>> options,
            ILogger<WorkTimeContext> logger, IServiceProvider serviceProvider) : base(options, logger)
        {
            _serviceProvider = serviceProvider;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Model.Base.Models.WorkTime.WorkTime>().Property(wt => wt.Holidays).HasConversion(
                e => e.ToJsonString(), json => json.HolidayEntryFromJsonString(),
                new ValueComparer<List<HolidayEntry>>(
                    (l1, l2) => l1!.ToJsonString().Equals(l2!.ToJsonString()),
                    l => l.ToJsonString().GetHashCode(),
                    l => l.ToList()));
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

        public async Task<Model.Base.Models.WorkTime.WorkTime?> GetActiveConfig()
        {
            var entity = await Entities.FirstOrDefaultAsync(wt => wt.Deleted == false);
            return entity;
        }
    }
}