using HR4You.Model.Base.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HR4You.Context
{
    public class RoleContext : DbContext
    {
        public RoleContext(DbContextOptions<RoleContext> options) : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }

        public async Task<Role> GetRoleByName(string name)
        {
            return await Roles.FirstOrDefaultAsync(u => u.Name == name);
        }

        public class RoleContextDesignTimeFactory : IDesignTimeDbContextFactory<RoleContext>
        {
            public RoleContext CreateDbContext(string[] args)
            {
                var builder = new DbContextOptionsBuilder<RoleContext>();
                var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");
                builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                return new RoleContext(builder.Options);
            }
        }
    }

}