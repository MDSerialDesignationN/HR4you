using HR4You.Contexts;
using HR4You.Contexts.Project;
using HR4You.Model.Base;
using HR4You.Model.Base.Models;
using HR4You.Model.Base.Models.Project;
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
    
    [HttpGet("get-all")]
    [SwaggerOperation("GetAllProjects")]
    //[Authorize(Policy = )]
    public async Task<IActionResult> GetAllProjects(bool addDeleted)
    {
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<ProjectContext>()!;
        
        var result = await sc.GetAll(addDeleted);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(),
            _ => BadRequest("something bad happened")
        };
    }
    
    [HttpGet("get")]
    [SwaggerOperation("GetProject")]
    //[Authorize(Policy = )]
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
    //[Authorize(Policy = )]
    public async Task<IActionResult> CreateProject([FromBody]Project project)
    {
        var checkResult = await _checker.CheckMasterData(project);
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
    //[Authorize(Policy = )]
    public async Task<IActionResult> EditProject(int id, [FromBody] Project project)
    {
        var checkResult = await _checker.CheckMasterData(project);
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
    //[Authorize(Policy = )]
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