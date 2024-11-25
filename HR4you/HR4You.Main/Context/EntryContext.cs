using HR4You.Model.Base.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HR4You.Context
{
    public class EntryContext : DbContext
    {
        public EntryContext(DbContextOptions<EntryContext> options) : base(options)
        {
        }

        public DbSet<Entry> Entries { get; set; }

        public async Task<Entry> GetEntryById(string id)
        {
            return await Entries.FirstOrDefaultAsync(u => u.Id == id);
        }

        public class EntryContextDesignTimeFactory : IDesignTimeDbContextFactory<EntryContext>
        {
            public EntryContext CreateDbContext(string[] args)
            {
                var builder = new DbContextOptionsBuilder<EntryContext>();
                var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");
                builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                return new EntryContext(builder.Options);
            }
        }
    }
}