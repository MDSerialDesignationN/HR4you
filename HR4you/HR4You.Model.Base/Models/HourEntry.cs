namespace HR4You.Model.Base.Models
{
    public class HourEntry : ModelBase
    {
        public required string UserId { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public int? Duration { get; set; }
        
        public int CustomerId { get; set; }
        public int ProjectId { get; set; }
        public int TaskId { get; set; }
        public int? FlagId { get; set; }
        
        public string? Description { get; set; } = string.Empty;
        
        bool IsHoliday { get; set; }
        bool IsBillable { get; set; }

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
            TaskId = data.TaskId;
            FlagId = data.FlagId;
            
            Description = data.Description;

            IsHoliday = data.IsHoliday;
            IsBillable = data.IsBillable;
        }
    }
}