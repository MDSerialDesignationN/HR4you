using HR4You.Main.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HR4You.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets für Entitäten, z.B. User, Product, etc.
        public DbSet<User> Users { get; set; }
    }
    
    public class ContractContextDesignTimeFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            optionsBuilder.UseMySql(args[0], ServerVersion.AutoDetect(args[0]));
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}