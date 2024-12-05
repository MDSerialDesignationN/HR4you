using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HR4You.Contexts.Project
{
    public class ProjectContext : ModelBaseContext<Model.Base.Models.Project.Project>
    {
        private readonly IServiceProvider _serviceProvider;

        public ProjectContext(DbContextOptions<ModelBaseContext<Model.Base.Models.Project.Project>> options,
            ILogger<ProjectContext> logger, IServiceProvider serviceProvider) : base(options, logger)
        {
            _serviceProvider = serviceProvider;
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