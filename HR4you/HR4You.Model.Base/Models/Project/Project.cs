using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HR4You.Model.Base.Models.Project
{
    [Table("hr4you_project")]
    public class Project : ModelBase
    {
        [Required] public int ProjectNumber { get; set; }

        [Required] public int CustomerId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(CustomerId))]
        public Customer.Customer? Customer { get; set; } = null!;

        [Required] public string Name { get; set; } = string.Empty;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required] public ProjectState State { get; set; }
        public string? Description { get; set; }

        [JsonIgnore] public ICollection<HourEntry.HourEntry>? HourEntries { get; } = new List<HourEntry.HourEntry>();

        public override void Set(ModelBase model)
        {
            var data = model as Project;
            if (data == null)
            {
                throw new ArgumentException("called with wrong type -> should be Project");
            }

            ProjectNumber = data.ProjectNumber;
            CustomerId = data.CustomerId;
            Name = data.Name;
            State = data.State;
            Description = data.Description;
        }
    }

    public enum ProjectState
    {
        Open,
        Closed
    }
}