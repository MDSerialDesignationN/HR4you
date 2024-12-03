using HR4You.Contexts;
using HR4You.Contexts.WorkTime;
using HR4You.Model.Base;
using HR4You.Model.Base.Models.WorkTime;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HR4You.Controllers;

[Route("/api/master-data/work-time")]
[ApiController]
public class WorkTimeController : ControllerBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ModelChecker _checker;

    public WorkTimeController(IServiceProvider serviceProvider, ModelChecker checker)
    {
        _serviceProvider = serviceProvider;
        _checker = checker;
    }
    
    [HttpGet("get-all")]
    [SwaggerOperation("GetAllWorkTimes")]
    //[Authorize(Policy = )]
    public async Task<IActionResult> GetAllWorkTimes(bool addDeleted)
    {
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<WorkTimeContext>()!;
        
        var result = await sc.GetAll(addDeleted);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(),
            _ => BadRequest("something bad happened")
        };
    }
    
    [HttpPost("create")]
    [SwaggerOperation("CreateWorkTime")]
    //[Authorize(Policy = )]
    public async Task<IActionResult> CreateWorkTime([FromBody]WorkTime workTime)
    {
        var checkResult = await _checker.CheckMasterData(workTime);
        if (checkResult.Error != ModelChecker.ModelCheckError.None)
        {
            return BadRequest(checkResult);
        }
        
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<WorkTimeContext>()!;
        
        var result = await sc.Create(workTime);
        if (result.Error == MasterDataError.None)
        {
            return Ok(result.Entity);
        }
        return BadRequest("something bad happened");
    }
}