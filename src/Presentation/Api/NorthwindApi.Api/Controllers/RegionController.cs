using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Api.Handler;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.DTOs.Region;
using NorthwindApi.Application.Features.Region.Commands;
using NorthwindApi.Application.Features.Region.Queries;

namespace NorthwindApi.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RegionsController(Dispatcher dispatcher) : ControllerBase
{
    [HttpGet("get-all")]
    public async Task<IActionResult> GetRegions()
    {
        var data = await dispatcher.DispatchAsync(new GetRegions());
        return this.ReturnActionHandler(data);
    }
    
    [HttpGet("get/{id:int}")]
    public async Task<IActionResult> GetRegion(int id)
    {
        var data = await dispatcher.DispatchAsync(new GetRegionById(id));
        return this.ReturnActionHandler(data);
    }
    
    [HttpPost("create-new")]
    public async Task<IActionResult> CreateRegion([FromBody]CreateRegionRequest createRegionRequest)
    {
        var data = await dispatcher.DispatchAsync(new CreateRegionCommand(createRegionRequest));
        return this.ReturnActionHandler(data);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateRegion([FromBody]UpdateRegionRequest request)
    {
        var data = await dispatcher.DispatchAsync(new UpdateRegionCommand(request));
        return this.ReturnActionHandler(data);
    }
    
    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> DeleteRegion(int id)
    {
        var data = await dispatcher.DispatchAsync(new DeleteRegionCommand(id));
        return this.ReturnActionHandler(data);
    }
}