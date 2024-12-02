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

    public async Task<MasterDataCheckResult> CheckMasterData(ModelBase data, int? id)
    {
        //TODO
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