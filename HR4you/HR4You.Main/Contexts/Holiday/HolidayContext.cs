using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HR4You.Contexts.Holiday
{
    public class HolidayContext : ModelBaseContext<Model.Base.Models.Holiday.Holiday>
    {
        public HolidayContext(DbContextOptions<ModelBaseContext<Model.Base.Models.Holiday.Holiday>> options,
            ILogger<HolidayContext> logger, IServiceProvider serviceProvider) : base(options, logger)
        {
        }

        public async Task<Model.Base.Models.Holiday.Holiday?> GetEntryForDate(DateOnly date)
        {
            var holiday = await Entities.Where(h => h.Date == date && h.Deleted == false).FirstOrDefaultAsync();

            return holiday;
        }
        
        public async Task<bool> IsDateFree(DateOnly holidayDate, int? id)
        {
            var exists = await Entities.Where(h => h.Deleted == false  && h.Id != id).Select(h => h.Date)
                .AnyAsync(date => date == holidayDate);

            return exists;
        }

        public class HolidayContextDesignTimeFactory : IDesignTimeDbContextFactory<HolidayContext>
        {
            public HolidayContext CreateDbContext(string[] args)
            {
                var builder = new DbContextOptionsBuilder<ModelBaseContext<Model.Base.Models.Holiday.Holiday>>();

                builder.UseMySql(args[0], ServerVersion.AutoDetect(args[0]));
                return new HolidayContext(builder.Options, null!, null!);
            }
        }
    }
}