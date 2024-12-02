namespace HR4You.Model.Base.Models.Customer
{
    public class Customer : ModelBase
    {
        public int CustomerNumber { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string? Address { get; set; }

        public string? Email { get; set; }

        public string? Website { get; set; }

        public string? PhoneNumber { get; set; }



        public override void Set(ModelBase model)
        {
            var data = model as Customer;
            if (data == null)
            {
                throw new ArgumentException("called with wrong type -> should be Customer");
            }

            CustomerNumber = data.CustomerNumber;
            Name = data.Name;
            Description = data.Description;
            Address = data.Address;
            Email = data.Email;
            Website = data.Website;
            PhoneNumber = data.PhoneNumber;
        }
    }
}