using HR4You.Contexts;
using HR4You.Contexts.Customer;
using HR4You.Model.Base;
using HR4You.Model.Base.Models.Customer;
using HR4You.Model.Base.Pagination;
using HR4you.Security;
using Microsoft.AspNetCore.Authorization;
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
    
    [HttpGet("get-all-paged")]
    [SwaggerOperation("GetAllPagedCustomers")]
    [Authorize(Policy = BuildInUserRoles.Authenticated)]
    public async Task<IActionResult> GetAllPagedCustomers([FromQuery] List<ColumnFilter> columnFilters, bool addDeleted, int pageNumber = 1, int pageSize = 10)
    {
        if (pageNumber <= 0 || pageSize <= 0)
            return BadRequest($"{nameof(pageNumber)} and {nameof(pageSize)} size must be greater than 0");
        
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<CustomerContext>()!;
        
        var result = await sc.GetAllOffsetPaged(columnFilters, pageNumber, pageSize, addDeleted);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(),
            _ => BadRequest("something bad happened")
        };
    }
    
    [HttpGet("get")]
    [SwaggerOperation("GetCustomer")]
    [Authorize(Policy = BuildInUserRoles.Authenticated)]
    public async Task<IActionResult> GetCustomer(int id, bool addDeleted)
    {
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<CustomerContext>()!;
        
        var result = await sc.Get(id, addDeleted);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(),
            _ => BadRequest("something bad happened")
        };
    }
    
    [HttpPost("create")]
    [SwaggerOperation("CreateCustomer")]
    [Authorize(Policy = BuildInUserRoles.AdminRole)]
    public async Task<IActionResult> CreateCustomer([FromBody]Customer customer)
    {
        var checkResult = await _checker.CheckMasterData(customer, null);
        if (checkResult.Error != ModelChecker.ModelCheckError.None)
        {
            return BadRequest(checkResult);
        }
        
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<CustomerContext>()!;
        
        var result = await sc.Create(customer);
        if (result.Error == MasterDataError.None)
        {
            return Ok(result.Entity);
        }
        return BadRequest("something bad happened");
    }

    [HttpPost("edit")]
    [SwaggerOperation("EditCustomer")]
    [Authorize(Policy = BuildInUserRoles.AdminRole)]
    public async Task<IActionResult> EditCustomer(int id, [FromBody] Customer customer)
    {
        var checkResult = await _checker.CheckMasterData(customer, id);
        if (checkResult.Error != ModelChecker.ModelCheckError.None)
        {
            return BadRequest(checkResult);
        }

        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<CustomerContext>()!;
        
        var result = await sc.Edit(id, customer);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(id),
            _ => BadRequest("something bad happened")
        };
    }

    [HttpDelete("delete")]
    [SwaggerOperation("DeleteCustomer")]
    [Authorize(Policy = BuildInUserRoles.AdminRole)]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        using var scope = _serviceProvider.CreateScope();
        var sc = scope.ServiceProvider.GetService<CustomerContext>()!;
        
        var result = await sc.SetDelete(id, true);
        return result.Error switch
        {
            MasterDataError.None => Ok(result.Entity),
            MasterDataError.NotFound => NotFound(id),
            _ => BadRequest("something bad happened")
        };
    }
}