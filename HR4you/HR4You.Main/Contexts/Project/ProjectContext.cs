using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HR4You.Contexts.Project
{
    public class ProjectContext : ModelBaseContext<Model.Base.Models.Project.Project>
    {
        public ProjectContext(DbContextOptions<ModelBaseContext<Model.Base.Models.Project.Project>> options,
            ILogger<ProjectContext> logger, IServiceProvider serviceProvider) : base(options, logger)
        {
        }

        public async Task<bool> IsProjectNumberFree(int projectNumber, int? id)
        {
            var exists = await Entities.Where(p => p.Deleted == false  && p.Id != id).Select(p => p.ProjectNumber)
                .AnyAsync(number => number == projectNumber);
            
            return exists;
        }

        public class ProjectContextDesignTimeFactory : IDesignTimeDbContextFactory<ProjectContext>
        {
            public ProjectContext CreateDbContext(string[] args)
            {
                var builder = new DbContextOptionsBuilder<ModelBaseContext<Model.Base.Models.Project.Project>>();

                builder.UseMySql(args[0], ServerVersion.AutoDetect(args[0]));
                return new ProjectContext(builder.Options, null!, null!);
            }
        }
    }
}