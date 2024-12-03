using HR4You.Model.Base.Models.Filter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HR4You.Contexts.Filter
{
    public class FilterContext : ModelBaseContext<Model.Base.Models.Filter.Filter>
    {
        private readonly IServiceProvider _serviceProvider;

        public FilterContext(DbContextOptions<ModelBaseContext<Model.Base.Models.Filter.Filter>> options,
            ILogger<FilterContext> logger, IServiceProvider serviceProvider) : base(options, logger)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<List<Model.Base.Models.Filter.Filter>> GetFilters(string? name, FilterType? type)
        {
            using var scope = _serviceProvider.CreateScope();
            var linq = Entities.AsQueryable();

            if (name != null)
            {
                linq = linq.Where(he => he.Name == name);
            }

            if (type != null)
            {
                linq = linq.Where(he => he.Type == type);
            }

            var list = await linq.ToListAsync();
            list = list.OrderByDescending(he => he.LastModifiedAt ?? he.CreationDateTime).ToList();

            return list;
        }

        public class EntryContextDesignTimeFactory : IDesignTimeDbContextFactory<FilterContext>
        {
            public FilterContext CreateDbContext(string[] args)
            {
                var builder = new DbContextOptionsBuilder<ModelBaseContext<Model.Base.Models.Filter.Filter>>();

                builder.UseMySql(args[0], ServerVersion.AutoDetect(args[0]));
                return new FilterContext(builder.Options, null!, null!);
            }
        }
    }
}