using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Api.Handler;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.DTOs.Territory;
using NorthwindApi.Application.Features.Territory.Commands;
using NorthwindApi.Application.Features.Territory.Queries;

namespace NorthwindApi.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TerritoriesController(Dispatcher dispatcher) : ControllerBase
{
    [HttpGet("get-all")]
    public async Task<IActionResult> GetTerritories()
    {
        var data = await dispatcher.DispatchAsync(new GetTerritories());
        return this.ReturnActionHandler(data);
    }
    
    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetTerritory(string id)
    {
        var data = await dispatcher.DispatchAsync(new GetTerritoryById(id));
        return this.ReturnActionHandler(data);
    }
    
    [HttpPost("create-new")]
    public async Task<IActionResult> CreateTerritory([FromBody]CreateTerritoryRequest createTerritoryRequest)
    {
        var data = await dispatcher.DispatchAsync(new CreateTerritoryCommand(createTerritoryRequest));
        return this.ReturnActionHandler(data);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateTerritory([FromBody]UpdateTerritoryRequest request)
    {
        var data = await dispatcher.DispatchAsync(new UpdateTerritoryCommand(request));
        return this.ReturnActionHandler(data);
    }
    
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteTerritory(string id)
    {
        var data = await dispatcher.DispatchAsync(new DeleteTerritoryCommand(id));
        return this.ReturnActionHandler(data);
    }
}