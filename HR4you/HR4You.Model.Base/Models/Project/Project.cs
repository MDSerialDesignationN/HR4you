namespace HR4You.Model.Base.Models.Project
{
    public class Project : ModelBase
    {
        public int ProjectNumber { get; set; }

        public int CustomerId { get; set; }

        public string Name { get; set; } = string.Empty;
        
        public ProjectState State { get; set; }
        
        public string? Description { get; set; }

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