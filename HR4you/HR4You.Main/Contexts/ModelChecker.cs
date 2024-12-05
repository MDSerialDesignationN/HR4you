using HR4You.Model.Base;

namespace HR4You.Contexts;

public class ModelChecker
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ModelChecker> _logger;
    
    public ModelChecker(IServiceProvider serviceProvider, ILogger<ModelChecker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task<MasterDataCheckResult> CheckMasterData(ModelBase data)
    {
        //TODO
        
        return MasterDataCheckResult.Ok();
        return data switch
        {
            Model.Base.Models.Customer.Customer customer => await CheckCustomer(customer),
            Model.Base.Models.HourEntry.HourEntry hourEntry => await CheckHourEntry(hourEntry),
            Model.Base.Models.WorkTime.WorkTime workTime => await CheckWorkTime(workTime),
            _ => throw new ArgumentException("unknown master-data type")
        };
    }

    private async Task<MasterDataCheckResult> CheckWorkTime(Model.Base.Models.WorkTime.WorkTime workTime)
    {
        _logger.LogDebug("Checking WorkTime");
        
        return MasterDataCheckResult.Ok();
    }

    private async Task<MasterDataCheckResult> CheckHourEntry(Model.Base.Models.HourEntry.HourEntry hourEntry)
    {
        _logger.LogDebug("Checking HourEntry");

        return MasterDataCheckResult.Ok();
    }

    private async Task<MasterDataCheckResult> CheckCustomer(Model.Base.Models.Customer.Customer customer)
    {
        _logger.LogDebug("Checking Customer");
        
        return MasterDataCheckResult.Ok();
    }

    public class MasterDataCheckResult
    {
        public ModelCheckError Error { get; set; }
        public List<string> AdditionalInfo { get; set; } = new();

        public static MasterDataCheckResult Ok()
        {
            return new MasterDataCheckResult();
        }
        public static MasterDataCheckResult NotOk(ModelCheckError error, List<string> info)
        {
            return new MasterDataCheckResult
            {
                Error = error,
                AdditionalInfo = info
            };
        }
    
    }
    
    public enum ModelCheckError
    {
        None
    }

}