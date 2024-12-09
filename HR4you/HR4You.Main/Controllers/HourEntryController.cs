using HR4You.Contexts;
using HR4You.Contexts.HourEntry;
using HR4You.Model.Base;
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
    public async Task<IActionResult> GetHourEntriesUser(bool addDeleted, string userId)
    {
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<HourEntryContext>()!;
        
        var result = await sc.GetHourEntries(addDeleted, userId);
        return Ok(result);
    }
    
    [HttpGet("get")]
    [SwaggerOperation("GetHourEntry")]
    //[Authorize(Policy = )]
    public async Task<IActionResult> GetHourEntry(int id, bool addDeleted)
    {
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<HourEntryContext>()!;
        
        var result = await sc.Get(id, addDeleted);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(),
            _ => BadRequest("something bad happened")
        };
    }
    
    [HttpPost("create")]
    [SwaggerOperation("CreateHourEntry")]
    //[Authorize(Policy = )]
    public async Task<IActionResult> CreateHourEntry([FromBody]HourEntry hourEntry)
    {
        var checkResult = await _checker.CheckMasterData(hourEntry);
        if (checkResult.Error != ModelChecker.ModelCheckError.None)
        {
            return BadRequest(checkResult);
        }
        
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<HourEntryContext>()!;
        
        var result = await sc.Create(hourEntry);
        if (result.Error == MasterDataError.None)
        {
            return Ok(result.Entity);
        }
        return BadRequest("something bad happened");
    }
    
    [HttpPost("edit")]
    [SwaggerOperation("EditHourEntry")]
    //[Authorize(Policy = )]
    public async Task<IActionResult> EditHourEntry(int id, [FromBody] HourEntry hourEntry)
    {
        var checkResult = await _checker.CheckMasterData(hourEntry);
        if (checkResult.Error != ModelChecker.ModelCheckError.None)
        {
            return BadRequest(checkResult);
        }

        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<HourEntryContext>()!;
        
        var result = await sc.Edit(id, hourEntry);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(id),
            _ => BadRequest("something bad happened")
        };
    }

    [HttpDelete("delete")]
    [SwaggerOperation("DeleteHourEntry")]
    //[Authorize(Policy = )]
    public async Task<IActionResult> DeleteHourEntry(int id)
    {
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<HourEntryContext>()!;
        
        var result = await sc.SetDelete(id, true);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(id),
            _ => BadRequest("something bad happened")
        };
    }
}