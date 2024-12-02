using System.Text.Json;

namespace HR4You.Model.Base.Models.WorkTime;

public class HolidayEntry
{
    public DateOnly Date { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool HalfDay { get; set; }
}

public static class HolidayEntryExtensions
{
    public static string ToJsonString(this List<HolidayEntry> entries)
    {
        return JsonSerializer.Serialize(entries);
    }

    public static List<HolidayEntry> HolidayEntryFromJsonString(this string json)
    {
        var list = JsonSerializer.Deserialize<List<HolidayEntry>>(json)!;
        return list;
    }
}