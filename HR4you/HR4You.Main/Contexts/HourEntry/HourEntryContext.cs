using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HR4You.Contexts.HourEntry
{
    public class HourEntryContext : ModelBaseContext<Model.Base.Models.HourEntry>
    {
        private readonly IServiceProvider _serviceProvider;

        public HourEntryContext(DbContextOptions<ModelBaseContext<Model.Base.Models.HourEntry>> options,
            ILogger<HourEntryContext> logger, IServiceProvider serviceProvider) : base(options, logger)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<List<Model.Base.Models.HourEntry>> GetHourEntries(bool addDeleted, string userId, int? customerId,
            int? projectId, int? taskId, int? flagId)
        {
            using var scope = _serviceProvider.CreateScope();
            var linq = Entities.AsQueryable();
            
            linq = linq.Where(he => he.UserId == userId);
            linq = linq.Where(he => he.Deleted == addDeleted);
            
            if (customerId != null)
            {
                linq = linq.Where(he => he.CustomerId == customerId);
            }
            if (projectId != null)
            {
                linq = linq.Where(he => he.ProjectId == projectId);
            }
            if (taskId != null)
            {
                linq = linq.Where(he => he.TaskId == taskId);
            }
            if (flagId != null)
            {
                linq = linq.Where(he => he.FlagId == flagId);
            }

            var list = await linq.ToListAsync();
            list = list.OrderByDescending(he => he.LastModifiedAt ?? he.CreationDateTime).ToList();

            return list;
        }

        public class EntryContextDesignTimeFactory : IDesignTimeDbContextFactory<HourEntryContext>
        {
            public HourEntryContext CreateDbContext(string[] args)
            {
                var builder = new DbContextOptionsBuilder<ModelBaseContext<Model.Base.Models.HourEntry>>();

                builder.UseMySql(args[0], ServerVersion.AutoDetect(args[0]));
                return new HourEntryContext(builder.Options, null!, null!);
            }
        }
    }
}