using HR4you.Security.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;

namespace HR4you.Security.Contexts
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var valueComparer = new ValueComparer<string[]>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c);

            modelBuilder.Entity<User>()
                .HasMany(e => e.RoleGroups)
                .WithMany(e => e.UserIds)
                .UsingEntity("userToRoleGroups");

            modelBuilder
                .Entity<UserRoleGroup>()
                .Property(e => e.Roles)
                .HasConversion(
                    v => v == null ? null : string.Join(',', v),
                    v => v == null ? null : v.Split(',', StringSplitOptions.RemoveEmptyEntries));

            modelBuilder
                .Entity<UserRoleGroup>()
                .Property(e => e.Roles)
                .Metadata
                .SetValueComparer(valueComparer);
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

                builder.UseMySql(args[0], ServerVersion.AutoDetect(args[0]));
                return new UserContext(builder.Options);
            }
        }
    }

}