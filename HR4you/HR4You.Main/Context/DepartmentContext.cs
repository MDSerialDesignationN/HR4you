using HR4You.Model.Base.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HR4You.Context
{
    public class DepartmentContext : DbContext
    {
        public DepartmentContext(DbContextOptions<DepartmentContext> options) : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }

        public async Task<Department> GetDepartmentById(string id)
        {
            return await Departments.FirstOrDefaultAsync(u => u.Id == id);
        }

        public class DepartmentContextDesignTimeFactory : IDesignTimeDbContextFactory<DepartmentContext>
        {
            public DepartmentContext CreateDbContext(string[] args)
            {
                var builder = new DbContextOptionsBuilder<DepartmentContext>();
                var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");
                builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                return new DepartmentContext(builder.Options);
            }
        }
    }
}