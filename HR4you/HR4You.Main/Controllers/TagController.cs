using HR4You.Contexts;
using HR4You.Contexts.Tag;
using HR4You.Model.Base;
using HR4You.Model.Base.Models.Tag;
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
    
    [HttpGet("get-all")]
    [SwaggerOperation("GetAllTags")]
    //[Authorize(Policy = )]
    public async Task<IActionResult> GetAllTags(bool addDeleted)
    {
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<TagContext>()!;
        
        var result = await sc.GetAll(addDeleted);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(),
            _ => BadRequest("something bad happened")
        };
    }
    
    [HttpPost("create")]
    [SwaggerOperation("CreateTag")]
    //[Authorize(Policy = )]
    public async Task<IActionResult> CreateTag([FromBody]Tag tag)
    {
        var checkResult = await _checker.CheckMasterData(tag);
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
    //[Authorize(Policy = )]
    public async Task<IActionResult> EditTag(int id, [FromBody] Tag tag)
    {
        var checkResult = await _checker.CheckMasterData(tag);
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
    //[Authorize(Policy = )]
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