using HR4You.Contexts;
using HR4You.Contexts.Customer;
using HR4You.Model.Base;
using HR4You.Model.Base.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HR4You.Controllers;

[Route("/api/master-data/customer")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ModelChecker _checker;

    public CustomerController(IServiceProvider serviceProvider, ModelChecker checker)
    {
        _serviceProvider = serviceProvider;
        _checker = checker;
    }
    
    [HttpGet("get-all")]
    [SwaggerOperation("GetAllCustomers")]
    //[Authorize(Policy = )]
    public async Task<IActionResult> GetAllCustomers(bool addDeleted)
    {
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<CustomerContext>()!;
        
        var result = await sc.GetAll(addDeleted);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(),
            _ => BadRequest("something bad happened")
        };
    }
    
    
    
    [HttpPost("create")]
    [SwaggerOperation("CreateCustomer")]
    //[Authorize(Policy = )]
    public async Task<IActionResult> CreateCustomer([FromBody]Customer he)
    {
        var checkResult = await _checker.CheckMasterData(he, null);
        if (checkResult.Error != ModelChecker.ModelCheckError.None)
        {
            return BadRequest(checkResult);
        }
        
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<CustomerContext>()!;
        
        var result = await sc.Create(he);
        if (result.Error == MasterDataError.None)
        {
            return Ok(result.Entity);
        }
        return BadRequest("something bad happened");
    }
}