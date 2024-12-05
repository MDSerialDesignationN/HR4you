using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HR4You.Contexts.Tag
{
    public class TagContext : ModelBaseContext<Model.Base.Models.Tag.Tag>
    {
        private readonly IServiceProvider _serviceProvider;

        public TagContext(DbContextOptions<ModelBaseContext<Model.Base.Models.Tag.Tag>> options,
            ILogger<TagContext> logger, IServiceProvider serviceProvider) : base(options, logger)
        {
            _serviceProvider = serviceProvider;
        }

        public class TagContextDesignTimeFactory : IDesignTimeDbContextFactory<TagContext>
        {
            public TagContext CreateDbContext(string[] args)
            {
                var builder = new DbContextOptionsBuilder<ModelBaseContext<Model.Base.Models.Tag.Tag>>();

                builder.UseMySql(args[0], ServerVersion.AutoDetect(args[0]));
                return new TagContext(builder.Options, null!, null!);
            }
        }
    }
}