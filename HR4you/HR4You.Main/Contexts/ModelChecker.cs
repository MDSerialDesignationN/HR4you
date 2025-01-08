using HR4You.Contexts.Customer;
using HR4You.Contexts.Holiday;
using HR4You.Contexts.Project;
using HR4You.Contexts.Tag;
using HR4You.Contexts.WorkTime;
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
        return data switch
        {
            Model.Base.Models.Customer.Customer customer => await CheckCustomer(customer, id),
            Model.Base.Models.Project.Project project => await CheckProject(project, id),
            Model.Base.Models.Tag.Tag tag => await CheckTag(tag, id),
            Model.Base.Models.WorkTime.WorkTime => await CheckWorkTime(id),
            Model.Base.Models.Holiday.Holiday holiday => await CheckHoliday(holiday, id),
            _ => throw new ArgumentException("unknown master-data type")
        };
    }
    
    private async Task<MasterDataCheckResult> CheckCustomer(Model.Base.Models.Customer.Customer customer, int? id)
    {
        _logger.LogDebug("Checking Customer");
        using var scope = _serviceProvider.CreateScope();
        var customerContext = scope.ServiceProvider.GetService<CustomerContext>()!;

        //Customer number is free
        var numberTaken = await customerContext.IsCustomerNumberFree(customer.CustomerNumber, id);
        if (numberTaken)
        {
            _logger.LogError("Customer number {Number} is already taken!", customer.CustomerNumber);
            return MasterDataCheckResult.NotOk(ModelCheckError.AlreadyExists,
                $"Customer number {customer.CustomerNumber} is already taken");
        }

        return MasterDataCheckResult.Ok();
    }

    private async Task<MasterDataCheckResult> CheckProject(Model.Base.Models.Project.Project project, int? id)
    {
        _logger.LogDebug("Checking Project");
        using var scope = _serviceProvider.CreateScope();
        var projectContext = scope.ServiceProvider.GetService<ProjectContext>()!;

        //Project number is free
        var numberTaken = await projectContext.IsProjectNumberFree(project.ProjectNumber, id);
        if (numberTaken)
        {
            _logger.LogError("Project number {Number} is already taken!", project.ProjectNumber);
            return MasterDataCheckResult.NotOk(ModelCheckError.AlreadyExists,
                $"Project number {project.ProjectNumber} is already taken");
        }
        
        // //Customer exists (not deleted) with given id
        var customerContext = scope.ServiceProvider.GetService<CustomerContext>()!;
        var customerResult = await customerContext.Get(project.CustomerId);
        if (customerResult.Error != MasterDataError.None)
        {
            _logger.LogError("Customer {Id} was not found!", project.CustomerId);

            return MasterDataCheckResult.NotOk(ModelCheckError.NotFound,
                $"Customer {project.CustomerId} was not found");
        }

        return MasterDataCheckResult.Ok();
    }

    private async Task<MasterDataCheckResult> CheckTag(Model.Base.Models.Tag.Tag tag, int? id)
    {
        _logger.LogDebug("Checking Tag");
        using var scope = _serviceProvider.CreateScope();
        var tagContext = scope.ServiceProvider.GetService<TagContext>()!;

        //Tag name is unique
        var nameTaken = await tagContext.IsTagNameFree(tag.Name, id);
        if (nameTaken)
        {
            _logger.LogError("Tag name {Name} is already taken!", tag.Name);
            return MasterDataCheckResult.NotOk(ModelCheckError.AlreadyExists, $"Tag name {tag.Name} is already taken!");
        }

        return MasterDataCheckResult.Ok();
    }

    private async Task<MasterDataCheckResult> CheckWorkTime(int? id)
    {
        _logger.LogDebug("Checking WorkTime");
        using var scope = _serviceProvider.CreateScope();
        var workTimeContext = scope.ServiceProvider.GetService<WorkTimeContext>()!;

        //No active config/No new config if 1 is active
        var activeConfig = await workTimeContext.GetActiveConfig();
        if (activeConfig != null && activeConfig.Id != id)
        {
            _logger.LogError("Work time config {Id} is active!", activeConfig.Id);

            return MasterDataCheckResult.NotOk(ModelCheckError.AlreadyExists,
                $"Work time config {activeConfig.Id} is active - only 1 allowed!");
        }

        return MasterDataCheckResult.Ok();
    }
    
    private async Task<MasterDataCheckResult> CheckHoliday(Model.Base.Models.Holiday.Holiday holiday, int? id)
    {
        _logger.LogDebug("Checking Holiday");
        using var scope = _serviceProvider.CreateScope();
        var holidayContext = scope.ServiceProvider.GetService<HolidayContext>()!;
        
        //Only one Holiday per date
        var isTaken = await holidayContext.IsDateFree(holiday.Date, id);
        if (isTaken)
        {
            _logger.LogError("Date {Date} is already taken by a holiday!", holiday.Date);
            
            return MasterDataCheckResult.NotOk(ModelCheckError.AlreadyExists, $"Date {holiday.Date} is already taken by a holiday");
        }
        
        return MasterDataCheckResult.Ok();
    }

    public class MasterDataCheckResult
    {
        public ModelCheckError Error { get; set; }
        public string AdditionalInfo { get; set; } = null!;

        public static MasterDataCheckResult Ok()
        {
            return new MasterDataCheckResult();
        }

        public static MasterDataCheckResult NotOk(ModelCheckError error, string info)
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
        None,
        NotFound,
        AlreadyExists
    }
}