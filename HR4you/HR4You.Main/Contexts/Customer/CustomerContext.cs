using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HR4You.Contexts.Customer
{
    public class CustomerContext : ModelBaseContext<Model.Base.Models.Customer.Customer>
    {
        private readonly IServiceProvider _serviceProvider;

        public CustomerContext(DbContextOptions<ModelBaseContext<Model.Base.Models.Customer.Customer>> options,
            ILogger<CustomerContext> logger, IServiceProvider serviceProvider) : base(options, logger)
        {
            _serviceProvider = serviceProvider;
        }

        public class EntryContextDesignTimeFactory : IDesignTimeDbContextFactory<CustomerContext>
        {
            public CustomerContext CreateDbContext(string[] args)
            {
                var builder = new DbContextOptionsBuilder<ModelBaseContext<Model.Base.Models.Customer.Customer>>();

                builder.UseMySql(args[0], ServerVersion.AutoDetect(args[0]));
                return new CustomerContext(builder.Options, null!, null!);
            }
        }
    }
}