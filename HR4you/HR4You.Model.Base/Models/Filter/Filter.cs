namespace HR4You.Model.Base.Models.Filter
{
    public class Filter : ModelBase
    {
        public string Name { get; set; } = string.Empty;
        
        public FilterType Type { get; set; }

        public override void Set(ModelBase model)
        {
            var data = model as Filter;
            if (data == null)
            {
                throw new ArgumentException("called with wrong type -> should be Filter");
            }

            Name = data.Name;
            Type = data.Type;
        }
    }
    
    public enum FilterType
    {
        Activity,
        Flag
    }
}