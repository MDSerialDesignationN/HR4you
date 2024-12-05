using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR4You.Model.Base.Models.WorkTime
{
    [Table("hr4you_worktime")]
    public class WorkTime : ModelBase
    {
        [Required] public int MinMonHours { get; set; }
        [Required] public int MinTueHours { get; set; }
        [Required] public int MinWedHours { get; set; }
        [Required] public int MinThuHours { get; set; }
        [Required] public int MinFriHours { get; set; }
        [Required] public int MinSatHours { get; set; }
        [Required] public int MinSunHours { get; set; }
        [JsonBlob] public List<HolidayEntry> Holidays { get; set; } = [];

        public override void Set(ModelBase model)
        {
            var data = model as WorkTime;
            if (data == null)
            {
                throw new ArgumentException("called with wrong type -> should be HourEntry");
            }

            MinMonHours = data.MinMonHours;
            MinTueHours = data.MinTueHours;
            MinWedHours = data.MinWedHours;
            MinThuHours = data.MinThuHours;
            MinFriHours = data.MinFriHours;
            MinSatHours = data.MinSatHours;
            MinSunHours = data.MinSunHours;
            Holidays = data.Holidays;
        }
    }
}