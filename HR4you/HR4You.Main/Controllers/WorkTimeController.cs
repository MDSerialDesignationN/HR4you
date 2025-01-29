using HR4You.Contexts;
using HR4You.Contexts.WorkTime;
using HR4You.Model.Base;
using HR4You.Model.Base.Models.WorkTime;
using HR4You.Model.Base.Pagination;
using HR4you.Security;
using Microsoft.AspNetCore.Authorization;
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
    
    [HttpGet("get-all-paged")]
    [SwaggerOperation("GetAllPagedWorkTimes")]
    [Authorize(Policy = BuildInUserRoles.Authenticated)]
    public async Task<IActionResult> GetAllPagedWorkTimes([FromQuery] List<ColumnFilter> columnFilters, bool addDeleted, int pageNumber = 1, int pageSize = 10)
    {
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<WorkTimeContext>()!;
        
        var result = await sc.GetAllOffsetPaged(columnFilters, pageNumber, pageSize, addDeleted);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(),
            _ => BadRequest("something bad happened")
        };
    }
    
    [HttpGet("get")]
    [SwaggerOperation("GetWorkTime")]
    // [Authorize(Policy = BuildInUserRoles.Authenticated)]
    public async Task<IActionResult> GetWorkTime(int id, bool addDeleted)
    {
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<WorkTimeContext>()!;
        
        var result = await sc.Get(id, addDeleted);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(),
            _ => BadRequest("something bad happened")
        };
    }
    
    [HttpPost("create")]
    [SwaggerOperation("CreateWorkTime")]
    // [Authorize(Policy = BuildInUserRoles.AdminRole)]
    public async Task<IActionResult> CreateWorkTime([FromBody]WorkTime workTime)
    {
        var checkResult = await _checker.CheckMasterData(workTime, null);
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
    
    [HttpPost("edit")]
    [SwaggerOperation("EditWorkTime")]
    // [Authorize(Policy = BuildInUserRoles.AdminRole)]
    public async Task<IActionResult> EditWorkTime(int id, [FromBody] WorkTime workTime)
    {
        var checkResult = await _checker.CheckMasterData(workTime, id);
        if (checkResult.Error != ModelChecker.ModelCheckError.None)
        {
            return BadRequest(checkResult);
        }

        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<WorkTimeContext>()!;
        
        var result = await sc.Edit(id, workTime);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(id),
            _ => BadRequest("something bad happened")
        };
    }

    [HttpDelete("delete")]
    [SwaggerOperation("DeleteWorkTime")]
    // [Authorize(Policy = BuildInUserRoles.AdminRole)]
    public async Task<IActionResult> DeleteWorkTime(int id)
    {
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<WorkTimeContext>()!;
        
        var result = await sc.SetDelete(id, true);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(id),
            _ => BadRequest("something bad happened")
        };
    }
}