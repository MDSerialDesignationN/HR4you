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
    
    //TODO finish with create endpoint checks on all different models
}