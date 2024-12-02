namespace HR4You.Model.Base.Models.WorkTime
{
    public class WorkTime : ModelBase
    {
        public int MinMonHours { get; set; }
        public int MinTueHours { get; set; }
        public int MinWedHours { get; set; }
        public int MinThuHours { get; set; }
        public int MinFriHours { get; set; }
        public int MinSatHours { get; set; }
        public int MinSunHours { get; set; }
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