using HR4You.Contexts.WorkTime;
using HR4You.Model.Base;
using HR4You.Model.Base.Models.HourEntry;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HR4You.Contexts.HourEntry
{
    public class HourEntryContext : ModelBaseContext<Model.Base.Models.HourEntry.HourEntry>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<HourEntryContext> _logger;

        public HourEntryContext(DbContextOptions<ModelBaseContext<Model.Base.Models.HourEntry.HourEntry>> options,
            ILogger<HourEntryContext> logger, IServiceProvider serviceProvider) : base(options, logger)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public new async Task<HourEntryResult> Create(
            Model.Base.Models.HourEntry.HourEntry hourEntry)
        {
            var result = await CalcProperties(hourEntry, _serviceProvider);
            if (result.Error != HourEntryError.None)
            {
                return result;
            }

            var dbResult = await base.Create(result.Entry!);
            return dbResult.Error != MasterDataError.None
                ? HourEntryResult.NotOk(HourEntryError.DbError, $"DB Error - {dbResult.Error}")
                : HourEntryResult.Ok(dbResult.Entity!);
        }

        public new async Task<HourEntryResult> Edit(int id, Model.Base.Models.HourEntry.HourEntry hourEntry)
        {
            var idOk = await Entities.FindAsync(id);
            if (idOk == null)
            {
                _logger.LogError("Hour entry with id {Id} was not found!", id);
                return HourEntryResult.NotOk(HourEntryError.DbError, id.ToString());
            }
            
            var result = await CalcProperties(hourEntry, _serviceProvider);
            if (result.Error != HourEntryError.None)
            {
                return result;
            }

            var dbResult = await base.Edit(id, hourEntry);
            return dbResult.Error != MasterDataError.None
                ? HourEntryResult.NotOk(HourEntryError.DbError,$"DB Error - {dbResult.Error}")
                : HourEntryResult.Ok(dbResult.Entity!);
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

        private async Task<HourEntryResult> CalcProperties(Model.Base.Models.HourEntry.HourEntry hourEntry,
            IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var workTimeContext = scope.ServiceProvider.GetService<WorkTimeContext>()!;
            var workTimeConfig = await workTimeContext.GetActiveConfig();

            if (workTimeConfig == null)
            {
                _logger.LogError("Work time config is empty or deleted");

                return HourEntryResult.NotOk(HourEntryError.NoConfig);
            }

            var hoursForDay = hourEntry.Date.DayOfWeek switch
            {
                DayOfWeek.Monday => workTimeConfig.MinMonHours,
                DayOfWeek.Tuesday => workTimeConfig.MinTueHours,
                DayOfWeek.Wednesday => workTimeConfig.MinWedHours,
                DayOfWeek.Thursday => workTimeConfig.MinThuHours,
                DayOfWeek.Friday => workTimeConfig.MinFriHours,
                DayOfWeek.Saturday => workTimeConfig.MinSatHours,
                DayOfWeek.Sunday => workTimeConfig.MinSunHours,
                _ => throw new ArgumentOutOfRangeException()
            };

            var holiday = workTimeConfig.GetEntryForDate(hourEntry.Date);
            if (holiday == null)
            {
                hourEntry.WorktimeDiff = hourEntry.Duration - hoursForDay;
            }
            else
            {
                hourEntry.IsHoliday = true;

                hourEntry.WorktimeDiff =
                    holiday.HalfDay
                        ? hourEntry.Duration - hoursForDay / 2
                        : hourEntry.Duration;
            }

            return HourEntryResult.Ok(hourEntry);
        }

        public class HourEntryContextDesignTimeFactory : IDesignTimeDbContextFactory<HourEntryContext>
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