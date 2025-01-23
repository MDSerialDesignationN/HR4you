using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HR4You.Model.Base.Models.Tag
{
    [Table("hr4you_tag")]
    public class Tag : ModelBase
    {
        [Required(ErrorMessage = "Name required!")] public string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<HourEntry.HourEntry>? HourEntries { get; } = new List<HourEntry.HourEntry>();

        public override void Set(ModelBase model)
        {
            var data = model as Tag;
            if (data == null)
            {
                throw new ArgumentException("called with wrong type -> should be Tag");
            }

            Name = data.Name;
        }
    }
}