using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR4You.Model.Base.Models.WorkTime
{
    [Table("hr4you_worktime")]
    public class WorkTime : ModelBase
    {
        /// <summary>
        /// format is hh.mm
        /// </summary>
        [Required] public float MinMonHours { get; set; }
        /// <summary>
        /// format is hh.mm
        /// </summary>
        [Required] public float MinTueHours { get; set; }
        /// <summary>
        /// format is hh.mm
        /// </summary>
        [Required] public float MinWedHours { get; set; }
        /// <summary>
        /// format is hh.mm
        /// </summary>
        [Required] public float MinThuHours { get; set; }
        /// <summary>
        /// format is hh.mm
        /// </summary>
        [Required] public float MinFriHours { get; set; }
        /// <summary>
        /// format is hh.mm
        /// </summary>
        [Required] public float MinSatHours { get; set; }
        /// <summary>
        /// format is hh.mm
        /// </summary>
        [Required] public float MinSunHours { get; set; }
        [JsonBlob] public List<HolidayEntry> Holidays { get; set; } = [];

        public HolidayEntry? GetEntryForDate(DateOnly dateOnly)
        {
            var result = Holidays.FirstOrDefault(he => he.Date == dateOnly);
            return result;
        }

        public override void Set(ModelBase model)
        {
            var data = model as WorkTime;
            if (data == null)
            {
                throw new ArgumentException("called with wrong type -> should be WorkTime");
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