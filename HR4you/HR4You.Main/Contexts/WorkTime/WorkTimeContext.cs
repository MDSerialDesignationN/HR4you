using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HR4You.Contexts.WorkTime
{
    public class WorkTimeContext : ModelBaseContext<Model.Base.Models.WorkTime>
    {
        private readonly IServiceProvider _serviceProvider;

        public WorkTimeContext(DbContextOptions<ModelBaseContext<Model.Base.Models.WorkTime>> options,
            ILogger<WorkTimeContext> logger, IServiceProvider serviceProvider) : base(options, logger)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<List<Model.Base.Models.WorkTime>> GetWorkTimeEntries(int? minMonHours, int? minTueHours,
            int? maxMonHours, int? minWedHours, int? minThuHours, int? minFriHours, int? minSatHours, int? minSunHours)
        {
            using var scope = _serviceProvider.CreateScope();
            var linq = Entities.AsQueryable();

            if (minMonHours != null)
            {
                linq = linq.Where(he => he.MinMonHours == minMonHours);
            }

            if (minTueHours != null)
            {
                linq = linq.Where(he => he.MinTueHours == minTueHours);
            }
            if (minWedHours != null)
            {
                linq = linq.Where(he => he.MinWedHours == minWedHours);
            }
            if (minThuHours != null)
            {
                linq = linq.Where(he => he.MinThuHours == minThuHours);
            }
            if (minFriHours != null)
            {
                linq = linq.Where(he => he.MinFriHours == minFriHours);
            }
            if (minSatHours != null)
            {
                linq = linq.Where(he => he.MinSatHours == minSatHours);
            }
            if (minSunHours != null)
            {
                linq = linq.Where(he => he.MinSunHours == minSunHours);
            }
            
            

            var list = await linq.ToListAsync();
            list = list.OrderByDescending(he => he.LastModifiedAt ?? he.CreationDateTime).ToList();

            return list;
        }

        public class EntryContextDesignTimeFactory : IDesignTimeDbContextFactory<WorkTimeContext>
        {
            public WorkTimeContext CreateDbContext(string[] args)
            {
                var builder = new DbContextOptionsBuilder<ModelBaseContext<Model.Base.Models.WorkTime>>();

                builder.UseMySql(args[0], ServerVersion.AutoDetect(args[0]));
                return new WorkTimeContext(builder.Options, null!, null!);
            }
        }
    }
}