using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HR4You.Model.Base.Models.Customer
{
    [Table("hr4you_customer")]
    public class Customer : ModelBase
    {
        [Required(ErrorMessage = "Customer number required!")] public int CustomerNumber { get; set; } //todo set constraint for unique
        [Required(ErrorMessage = "Name required!")] public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Address { get; set; }
        [EmailAddress(ErrorMessage = "Please enter a valid email address")] public string? Email { get; set; }
        public string? Website { get; set; }
        public string? PhoneNumber { get; set; }
        [JsonIgnore] public ICollection<Project.Project>? Projects { get; } = new List<Project.Project>();

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