using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HR4You.Contexts.Tag
{
    public class TagContext : ModelBaseContext<Model.Base.Models.Tag.Tag>
    {
        public TagContext(DbContextOptions<ModelBaseContext<Model.Base.Models.Tag.Tag>> options,
            ILogger<TagContext> logger, IServiceProvider serviceProvider) : base(options, logger)
        {
        }

        public async Task<bool> IsTagNameFree(string tagName, int? id)
        {
            var exists = await Entities.Where(t => t.Deleted == false && t.Id != id).Select(t => t.Name)
                .AnyAsync(name => name == tagName);

            return exists;
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