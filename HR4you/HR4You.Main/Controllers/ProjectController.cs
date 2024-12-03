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
    
    
    
    [HttpPost("create")]
    [SwaggerOperation("CreateProject")]
    //[Authorize(Policy = )]
    public async Task<IActionResult> CreateProject([FromBody]Project he)
    {
        var checkResult = await _checker.CheckMasterData(he, null);
        if (checkResult.Error != ModelChecker.ModelCheckError.None)
        {
            return BadRequest(checkResult);
        }
        
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<ProjectContext>()!;
        
        var result = await sc.Create(he);
        if (result.Error == MasterDataError.None)
        {
            return Ok(result.Entity);
        }
        return BadRequest("something bad happened");
    }
}