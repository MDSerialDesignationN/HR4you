using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HR4You.Contexts.HourEntry
{
    public class HourEntryContext : ModelBaseContext<Model.Base.Models.HourEntry.HourEntry>
    {
        private readonly IServiceProvider _serviceProvider;

        public HourEntryContext(DbContextOptions<ModelBaseContext<Model.Base.Models.HourEntry.HourEntry>> options,
            ILogger<HourEntryContext> logger, IServiceProvider serviceProvider) : base(options, logger)
        {
            _serviceProvider = serviceProvider;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public async Task<List<Model.Base.Models.HourEntry.HourEntry>> GetHourEntries(bool addDeleted, string userId)
        {
            using var scope = _serviceProvider.CreateScope();
            var linq = Entities.AsQueryable();
            
            linq = linq.Where(he => he.UserId == userId);
            linq = linq.Where(he => he.Deleted == addDeleted);

            var list = await linq.ToListAsync();
            list = list.OrderByDescending(he => he.LastModifiedAt ?? he.CreationDateTime).ToList();

            return list;
        }

        public class EntryContextDesignTimeFactory : IDesignTimeDbContextFactory<HourEntryContext>
        {
            public HourEntryContext CreateDbContext(string[] args)
            {
                var builder = new DbContextOptionsBuilder<ModelBaseContext<Model.Base.Models.HourEntry.HourEntry>>();

                builder.UseMySql(args[0], ServerVersion.AutoDetect(args[0]));
                return new HourEntryContext(builder.Options, null!, null!);
            }
        }
    }
}