
namespace HR4You.Model.Base.Models.HourEntry;

public class HourEntryResult
{
    public HourEntry? Entry { get; set; } 
    public HourEntryError Error { get; set; }
    public string? Details { get; set; }
    
    
    
    public static HourEntryResult NotOk(HourEntryError error)
    {
        return new HourEntryResult
        {
            Entry = null,
            Error = error
        };
    }
    
    public static HourEntryResult NotOk(HourEntryError error, string? details)
    {
        return new HourEntryResult
        {
            Entry = null,
            Error = error,
            Details = details
        };
    }
    
    public static HourEntryResult Ok(HourEntry hourEntry)
    {
        return new HourEntryResult
        {
            Entry = hourEntry,
            Error = HourEntryError.None
        };
    }
}

public enum HourEntryError
{
    None,
    DbError,
    NoConfig
}