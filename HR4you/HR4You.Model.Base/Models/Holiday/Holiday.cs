using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR4You.Model.Base.Models.Holiday;

[Table("hr4you_holiday")]
public class Holiday : ModelBase
{
    [Required(ErrorMessage = "Date required!")] public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    [Required(ErrorMessage = "Name required!")] public string Name { get; set; } = string.Empty;
    [Required] public bool HalfDay { get; set; }

    public override void Set(ModelBase model)
    {
        var data = model as Holiday;
        if (data == null)
        {
            throw new ArgumentException("called with wrong type -> should be Holiday");
        }

        Date = data.Date;
        Name = data.Name;
        HalfDay = data.HalfDay;
    }
}