using HR4You.Contexts;
using HR4You.Contexts.HourEntry;
using HR4You.Model.Base;
using HR4You.Model.Base.Models;
using HR4You.Model.Base.Models.HourEntry;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HR4You.Controllers;

[Route("/api/master-data/hour-entry")]
[ApiController]
public class HourEntryController : ControllerBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ModelChecker _checker;

    public HourEntryController(IServiceProvider serviceProvider, ModelChecker checker)
    {
        _serviceProvider = serviceProvider;
        _checker = checker;
    }
    
    [HttpGet("get-all")]
    [SwaggerOperation("GetAllHourEntries")]
    //[Authorize(Policy = )]
    public async Task<IActionResult> GetAllHourEntries(bool addDeleted)
    {
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<HourEntryContext>()!;
        
        var result = await sc.GetAll(addDeleted);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(),
            _ => BadRequest("something bad happened")
        };
    }
    
    [HttpGet("get-user-all")]
    [SwaggerOperation("GetHourEntriesUser")]
    //[Authorize(Policy = )]
    public async Task<IActionResult> GetHourEntriesUser(bool addDeleted, string userId, int? customerId,
        int? projectId, int? taskId, int? flagId)
    {
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<HourEntryContext>()!;
        
        var result = await sc.GetHourEntries(addDeleted, userId, customerId, projectId, taskId, flagId);
        return Ok(result);
    }
    
    [HttpPost("create")]
    [SwaggerOperation("CreateHourEntry")]
    //[Authorize(Policy = )]
    public async Task<IActionResult> CreateHourEntry([FromBody]HourEntry he)
    {
        var checkResult = await _checker.CheckMasterData(he, null);
        if (checkResult.Error != ModelChecker.ModelCheckError.None)
        {
            return BadRequest(checkResult);
        }
        
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<HourEntryContext>()!;
        
        var result = await sc.Create(he);
        if (result.Error == MasterDataError.None)
        {
            return Ok(result.Entity);
        }
        return BadRequest("something bad happened");
    }
}