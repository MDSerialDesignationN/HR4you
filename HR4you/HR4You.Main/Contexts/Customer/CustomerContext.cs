using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HR4You.Contexts.Customer
{
    public class CustomerContext : ModelBaseContext<Model.Base.Models.Customer>
    {
        private readonly IServiceProvider _serviceProvider;

        public CustomerContext(DbContextOptions<ModelBaseContext<Model.Base.Models.Customer>> options,
            ILogger<CustomerContext> logger, IServiceProvider serviceProvider) : base(options, logger)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<List<Model.Base.Models.Customer>> GetCustomers(int? customerNumber, string? name,
            string? description, string? address, string? email, string? website, string? phoneNumber)
        {
            using var scope = _serviceProvider.CreateScope();
            var linq = Entities.AsQueryable();
            
            if (customerNumber != null)
            {
                linq = linq.Where(he => he.CustomerNumber == customerNumber);
            }
            
            if (name != null)
            {
                linq = linq.Where(he => he.Name == name);
            }
            
            if (description != null)
            {
                linq = linq.Where(he => he.Description == description);
            }
            
            if (address != null)
            {
                linq = linq.Where(he => he.Address == address);
            }
            
            if (email != null)
            {
                linq = linq.Where(he => he.Email == email);
            }
            
            if (website != null)
            {
                linq = linq.Where(he => he.Website == website);
            }
            
            if (phoneNumber != null)
            {
                linq = linq.Where(he => he.PhoneNumber == phoneNumber);
            }
            
            var list = await linq.ToListAsync();
            list = list.OrderByDescending(he => he.LastModifiedAt ?? he.CreationDateTime).ToList();

            return list;
        }

        public class EntryContextDesignTimeFactory : IDesignTimeDbContextFactory<CustomerContext>
        {
            public CustomerContext CreateDbContext(string[] args)
            {
                var builder = new DbContextOptionsBuilder<ModelBaseContext<Model.Base.Models.Customer>>();

                builder.UseMySql(args[0], ServerVersion.AutoDetect(args[0]));
                return new CustomerContext(builder.Options, null!, null!);
            }
        }
    }
}