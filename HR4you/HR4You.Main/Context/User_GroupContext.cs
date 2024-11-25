using HR4You.Model.Base.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HR4You.Context
{
    public class User_GroupContext : DbContext
    {
        public User_GroupContext(DbContextOptions<User_GroupContext> options) : base(options)
        {
        }

        public DbSet<User_Group> User_Groups { get; set; }
        
        public class User_GroupContextDesignTimeFactory : IDesignTimeDbContextFactory<User_GroupContext>
        {
            public User_GroupContext CreateDbContext(string[] args)
            {
                var builder = new DbContextOptionsBuilder<User_GroupContext>();
                var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");
                builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                return new User_GroupContext(builder.Options);
            }
        }
    }

}