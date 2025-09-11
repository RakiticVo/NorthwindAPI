using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Api.Handler;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.DTOs.Shipper;
using NorthwindApi.Application.Features.Shipper.Commands;
using NorthwindApi.Application.Features.Shipper.Queries;

namespace NorthwindApi.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ShippersController(Dispatcher dispatcher) : ControllerBase
{
    [HttpGet("get-all")]
    public async Task<IActionResult> GetShippers()
    {
        var data = await dispatcher.DispatchAsync(new GetShippers());
        return this.ReturnActionHandler(data);
    }
    
    [HttpGet("get/{id:int}")]
    public async Task<IActionResult> GetShipper(int id)
    {
        var data = await dispatcher.DispatchAsync(new GetShipperById(id));
        return this.ReturnActionHandler(data);
    }
    
    [HttpPost("create-new")]
    public async Task<IActionResult> CreateShipper([FromBody]CreateShipperRequest request)
    {
        var data = await dispatcher.DispatchAsync(new CreateShipperCommand(request));
        return this.ReturnActionHandler(data);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateShipper([FromBody]UpdateShipperRequest request)
    {
        var data = await dispatcher.DispatchAsync(new UpdateShipperCommand(request));
        return this.ReturnActionHandler(data);
    }
    
    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> DeleteShipper(int id)
    {
        var data = await dispatcher.DispatchAsync(new DeleteShipperCommand(id));
        return this.ReturnActionHandler(data);
    }
}