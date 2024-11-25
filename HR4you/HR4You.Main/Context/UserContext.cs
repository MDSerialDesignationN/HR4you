using HR4You.Model.Base.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HR4You.Context
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public class UserContextDesignTimeFactory : IDesignTimeDbContextFactory<UserContext>
        {
            public UserContext CreateDbContext(string[] args)
            {
                var builder = new DbContextOptionsBuilder<UserContext>();
                var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");
                builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                return new UserContext(builder.Options);
            }
        }
    }

}