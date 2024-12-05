using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HR4You.Model.Base.Models.HourEntry
{
    [Table("hr4you_hourEntry")]
    public class HourEntry : ModelBase
    {
        [Required] public string UserId { get; set; } = null!;
        [Required] public TimeOnly StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public int? Duration { get; set; }
        [Required] public ActivityType Type { get; set; }
        [Required] public int CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        [JsonIgnore]
        public Customer.Customer? Customer { get; set; } = null!;

        [Required] public int ProjectId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        [JsonIgnore]
        public Project.Project? Project { get; set; } = null!;

        public int? TagId { get; set; }

        [ForeignKey(nameof(TagId))]
        [JsonIgnore]
        public Tag.Tag? Tag { get; set; } = null!;

        public string? Description { get; set; } = string.Empty;
        [Required] public bool IsHoliday { get; set; }
        [Required] public bool IsBillable { get; set; }

        public override void Set(ModelBase model)
        {
            var data = model as HourEntry;
            if (data == null)
            {
                throw new ArgumentException("called with wrong type -> should be HourEntry");
            }

            UserId = data.UserId;

            StartTime = data.StartTime;
            EndTime = data.EndTime;
            Duration = data.Duration;

            CustomerId = data.CustomerId;
            ProjectId = data.ProjectId;

            Type = data.Type;
            TagId = data.TagId;

            Description = data.Description;

            IsHoliday = data.IsHoliday;
            IsBillable = data.IsBillable;
        }
    }

    public enum ActivityType
    {
        General,
        Apprentice,
        Planning,
        Documentation,
        Hmi,
        Plc
    }
}