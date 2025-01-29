using HR4You.Contexts;
using HR4You.Contexts.Project;
using HR4You.Model.Base;
using HR4You.Model.Base.Models.Project;
using HR4You.Model.Base.Pagination;
using HR4you.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HR4You.Controllers;

[Route("/api/master-data/project")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ModelChecker _checker;

    public ProjectController(IServiceProvider serviceProvider, ModelChecker checker)
    {
        _serviceProvider = serviceProvider;
        _checker = checker;
    }
    
    [HttpGet("get-all-paged")]
    [SwaggerOperation("GetAllPagedProjects")]
    [Authorize(Policy = BuildInUserRoles.Authenticated)]
    public async Task<IActionResult> GetAllPagedProjects([FromQuery] List<ColumnFilter> columnFilters, bool addDeleted, int pageNumber = 1, int pageSize = 10)
    {
        if (pageNumber <= 0 || pageSize <= 0)
            return BadRequest($"{nameof(pageNumber)} and {nameof(pageSize)} size must be greater than 0");
        
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<ProjectContext>()!;
        
        var result = await sc.GetAllOffsetPaged(columnFilters, pageNumber, pageSize, addDeleted);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(),
            _ => BadRequest("something bad happened")
        };
    }
    
    [HttpGet("get")]
    [SwaggerOperation("GetProject")]
    // [Authorize(Policy = BuildInUserRoles.Authenticated)]
    public async Task<IActionResult> GetProject(int id, bool addDeleted)
    {
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<ProjectContext>()!;
        
        var result = await sc.Get(id, addDeleted);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(),
            _ => BadRequest("something bad happened")
        };
    }
    
    [HttpPost("create")]
    [SwaggerOperation("CreateProject")]
    // [Authorize(Policy = BuildInUserRoles.AdminRole)]
    public async Task<IActionResult> CreateProject([FromBody]Project project)
    {
        var checkResult = await _checker.CheckMasterData(project, null);
        if (checkResult.Error != ModelChecker.ModelCheckError.None)
        {
            return BadRequest(checkResult);
        }
        
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<ProjectContext>()!;
        
        var result = await sc.Create(project);
        if (result.Error == MasterDataError.None)
        {
            return Ok(result.Entity);
        }
        return BadRequest("something bad happened");
    }
    
    [HttpPost("edit")]
    [SwaggerOperation("EditProject")]
    // [Authorize(Policy = BuildInUserRoles.AdminRole)]
    public async Task<IActionResult> EditProject(int id, [FromBody] Project project)
    {
        var checkResult = await _checker.CheckMasterData(project, id);
        if (checkResult.Error != ModelChecker.ModelCheckError.None)
        {
            return BadRequest(checkResult);
        }

        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<ProjectContext>()!;
        
        var result = await sc.Edit(id, project);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(id),
            _ => BadRequest("something bad happened")
        };
    }

    [HttpDelete("delete")]
    [SwaggerOperation("DeleteProject")]
    // [Authorize(Policy = BuildInUserRoles.AdminRole)]
    public async Task<IActionResult> DeleteProject(int id)
    {
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<ProjectContext>()!;
        
        var result = await sc.SetDelete(id, true);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(id),
            _ => BadRequest("something bad happened")
        };
    }
}