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

    public HourEntryController(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    [HttpGet("get-all-paged")]
    [SwaggerOperation("GetAllPagedHourEntries")]
    //[Authorize(Policy = )]
    public async Task<IActionResult> GetAllPagedHourEntries(bool addDeleted, string? userId, int reference = 0, int pageSize = 10)
    {
        if (pageSize <= 0)
            return BadRequest($"{nameof(pageSize)} size must be greater than 0");
        
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<HourEntryContext>()!;
        
        var result = await sc.GetAllPagedEntries(reference, pageSize, addDeleted, userId);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(),
            _ => BadRequest("something bad happened")
        };
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
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<HourEntryContext>()!;
        
        var result = await sc.Create(hourEntry);
        if (result.Error == HourEntryError.None)
        {
            return Ok(result.Entry);
        }
        
        return BadRequest(result.Error);
    }
    
    [HttpPost("edit")]
    [SwaggerOperation("EditHourEntry")]
    //[Authorize(Policy = )]
    public async Task<IActionResult> EditHourEntry(int id, [FromBody] HourEntry hourEntry)
    {
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<HourEntryContext>()!;
        
        var result = await sc.Edit(id, hourEntry);
        if (result.Error == HourEntryError.None)
        {
            return Ok(result.Entry);
        }
        
        return BadRequest(result.Error);
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