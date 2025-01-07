using HR4You.Contexts;
using HR4You.Contexts.Holiday;
using HR4You.Model.Base;
using HR4You.Model.Base.Models.Holiday;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HR4You.Controllers;

[Route("/api/master-data/holiday")]
[ApiController]
public class HolidayController : ControllerBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ModelChecker _checker;

    public HolidayController(IServiceProvider serviceProvider, ModelChecker checker)
    {
        _serviceProvider = serviceProvider;
        _checker = checker;
    }
    
    [HttpGet("get-all-paged")]
    [SwaggerOperation("GetAllPagedHolidays")]
    //[Authorize(Policy = )]
    public async Task<IActionResult> GetAllPagedHolidays(bool addDeleted, int pageNumber = 1, int pageSize = 10)
    {
        if (pageNumber <= 0 || pageSize <= 0)
            return BadRequest($"{nameof(pageNumber)} and {nameof(pageSize)} size must be greater than 0");
        
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<HolidayContext>()!;
        
        var result = await sc.GetAllOffsetPaged(pageNumber, pageSize, addDeleted);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(),
            _ => BadRequest("something bad happened")
        };
    }
    
    [HttpGet("get")]
    [SwaggerOperation("GetHoliday")]
    //[Authorize(Policy = )]
    public async Task<IActionResult> GetHoliday(int id, bool addDeleted)
    {
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<HolidayContext>()!;
        
        var result = await sc.Get(id, addDeleted);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(),
            _ => BadRequest("something bad happened")
        };
    }
    
    [HttpPost("create")]
    [SwaggerOperation("CreateHoliday")]
    //[Authorize(Policy = )]
    public async Task<IActionResult> CreateHoliday([FromBody]Holiday holiday)
    {
        var checkResult = await _checker.CheckMasterData(holiday, null);
        if (checkResult.Error != ModelChecker.ModelCheckError.None)
        {
            return BadRequest(checkResult);
        }
        
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<HolidayContext>()!;
        
        var result = await sc.Create(holiday);
        if (result.Error == MasterDataError.None)
        {
            return Ok(result.Entity);
        }
        return BadRequest("something bad happened");
    }

    [HttpPost("edit")]
    [SwaggerOperation("EditHoliday")]
    //[Authorize(Policy = )]
    public async Task<IActionResult> EditHoliday(int id, [FromBody] Holiday holiday)
    {
        var checkResult = await _checker.CheckMasterData(holiday, id);
        if (checkResult.Error != ModelChecker.ModelCheckError.None)
        {
            return BadRequest(checkResult);
        }

        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<HolidayContext>()!;
        
        var result = await sc.Edit(id, holiday);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(id),
            _ => BadRequest("something bad happened")
        };
    }

    [HttpDelete("delete")]
    [SwaggerOperation("DeleteHoliday")]
    //[Authorize(Policy = )]
    public async Task<IActionResult> DeleteHoliday(int id)
    {
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<HolidayContext>()!;
        
        var result = await sc.SetDelete(id, true);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(id),
            _ => BadRequest("something bad happened")
        };
    }
}