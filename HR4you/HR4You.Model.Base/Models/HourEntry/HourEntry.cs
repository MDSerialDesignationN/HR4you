using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HR4You.Model.Base.Models.HourEntry
{
    [Table("hr4you_hourEntry")]
    public class HourEntry : ModelBase
    {
        [Required] public string UserId { get; set; } = null!;
        [Required] public DateOnly Date { get; set; }
        [Required] public TimeOnly StartTime { get; set; }
        [Required] public TimeOnly EndTime { get; set; }
        /// <summary>
        /// format is hh.mm
        /// </summary>
        [Required] public float Duration { get; set; }
        /// <summary>
        /// format is hh.mm
        /// </summary>
        public float WorktimeDiff { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required] public ActivityType Type { get; set; }
        [Required] public int ProjectId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        [JsonIgnore]
        public Project.Project? Project { get; set; } = null!;

        public int? TagId { get; set; }

        [ForeignKey(nameof(TagId))]
        [JsonIgnore]
        public Tag.Tag? Tag { get; set; } = null!;

        public string? Description { get; set; } = string.Empty;
        public bool IsHoliday { get; set; }
        [Required] public bool IsBillable { get; set; }

        public override void Set(ModelBase model)
        {
            var data = model as HourEntry;
            if (data == null)
            {
                throw new ArgumentException("called with wrong type -> should be HourEntry");
            }

            UserId = data.UserId;

            Date = data.Date;
            StartTime = data.StartTime;
            EndTime = data.EndTime;
            Duration = data.Duration;
            WorktimeDiff = data.WorktimeDiff;
            Type = data.Type;

            ProjectId = data.ProjectId;
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