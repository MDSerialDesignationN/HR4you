using HR4You.Contexts;
using HR4You.Contexts.Tag;
using HR4You.Model.Base;
using HR4You.Model.Base.Models.Tag;
using HR4You.Model.Base.Pagination;
using HR4you.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HR4You.Controllers;

[Route("/api/master-data/tag")]
[ApiController]
public class TagController : ControllerBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ModelChecker _checker;

    public TagController(IServiceProvider serviceProvider, ModelChecker checker)
    {
        _serviceProvider = serviceProvider;
        _checker = checker;
    }
    
    [HttpGet("get-all-paged")]
    [SwaggerOperation("GetAllPagedTags")]
    //[Authorize(Policy = BuildInUserRoles.Authenticated)]
    public async Task<IActionResult> GetAllPagedTags([FromQuery] List<ColumnFilter> columnFilters, bool addDeleted, int pageNumber = 1, int pageSize = 10)
    { 
        if (pageNumber <= 0 || pageSize <= 0)
            return BadRequest($"{nameof(pageNumber)} and {nameof(pageSize)} size must be greater than 0");
        
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<TagContext>()!;
        
        var result = await sc.GetAllOffsetPaged(columnFilters, pageNumber, pageSize, addDeleted);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(),
            _ => BadRequest("something bad happened")
        };
    }
    
    [HttpGet("get")]
    [SwaggerOperation("GetTag")]
    // [Authorize(Policy = BuildInUserRoles.Authenticated)]
    public async Task<IActionResult> GetTag(int id, bool addDeleted)
    {
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<TagContext>()!;
        
        var result = await sc.Get(id, addDeleted);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(),
            _ => BadRequest("something bad happened")
        };
    }
    
    [HttpPost("create")]
    [SwaggerOperation("CreateTag")]
    // [Authorize(Policy = BuildInUserRoles.AdminRole)]
    public async Task<IActionResult> CreateTag([FromBody]Tag tag)
    {
        var checkResult = await _checker.CheckMasterData(tag, null);
        if (checkResult.Error != ModelChecker.ModelCheckError.None)
        {
            return BadRequest(checkResult);
        }
        
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<TagContext>()!;
        
        var result = await sc.Create(tag);
        if (result.Error == MasterDataError.None)
        {
            return Ok(result.Entity);
        }
        return BadRequest("something bad happened");
    }
    
    [HttpPost("edit")]
    [SwaggerOperation("EditTag")]
    // [Authorize(Policy = BuildInUserRoles.AdminRole)]
    public async Task<IActionResult> EditTag(int id, [FromBody] Tag tag)
    {
        var checkResult = await _checker.CheckMasterData(tag, id);
        if (checkResult.Error != ModelChecker.ModelCheckError.None)
        {
            return BadRequest(checkResult);
        }

        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<TagContext>()!;
        
        var result = await sc.Edit(id, tag);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(id),
            _ => BadRequest("something bad happened")
        };
    }

    [HttpDelete("delete")]
    [SwaggerOperation("DeleteTag")]
    // [Authorize(Policy = BuildInUserRoles.AdminRole)]
    public async Task<IActionResult> DeleteTag(int id)
    {
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<TagContext>()!;
        
        var result = await sc.SetDelete(id, true);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(id),
            _ => BadRequest("something bad happened")
        };
    }
}