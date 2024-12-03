using HR4You.Model.Base.Models.Project;
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

        public async Task<List<Model.Base.Models.Project.Project>> GetProjects(int? projectNumber, int? customerId,
            string? name, ProjectState? state, string? description)
        {
            using var scope = _serviceProvider.CreateScope();
            var linq = Entities.AsQueryable();

            if (projectNumber != null)
            {
                linq = linq.Where(he => he.ProjectNumber == projectNumber);
            }

            if (customerId != null)
            {
                linq = linq.Where(he => he.CustomerId == customerId);
            }

            if (name != null)
            {
                linq = linq.Where(he => he.Name == name);
            }

            if (state != null)
            {
                linq = linq.Where(he => he.State == state);
            }

            if (description != null)
            {
                linq = linq.Where(he => he.Description == description);
            }

            var list = await linq.ToListAsync();
            list = list.OrderByDescending(he => he.LastModifiedAt ?? he.CreationDateTime).ToList();

            return list;
        }

        public class EntryContextDesignTimeFactory : IDesignTimeDbContextFactory<ProjectContext>
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