using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Api.Handler;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.DTOs.Supplier;
using NorthwindApi.Application.Features.Supplier.Commands;
using NorthwindApi.Application.Features.Supplier.Queries;

namespace NorthwindApi.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SuppliersController(Dispatcher dispatcher) : ControllerBase
{
    [HttpGet("get-all")]
    public async Task<IActionResult> GetSuppliers()
    {
        var data = await dispatcher.DispatchAsync(new GetSuppliers());
        return this.ReturnActionHandler(data);
    }
    
    [HttpGet("get/{id:int}")]
    public async Task<IActionResult> GetSupplier(int id)
    {
        var data = await dispatcher.DispatchAsync(new GetSupplierById(id));
        return this.ReturnActionHandler(data);
    }
    
    [HttpPost("create-new")]
    public async Task<IActionResult> CreateSupplier([FromBody]CreateSupplierRequest request)
    {
        var data = await dispatcher.DispatchAsync(new CreateSupplierCommand(request));
        return this.ReturnActionHandler(data);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateSupplier([FromBody]UpdateSupplierRequest request)
    {
        var data = await dispatcher.DispatchAsync(new UpdateSupplierCommand(request));
        return this.ReturnActionHandler(data);
    }
    
    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> DeleteSupplier(int id)
    {
        var data = await dispatcher.DispatchAsync(new DeleteSupplierCommand(id));
        return this.ReturnActionHandler(data);
    }
}
