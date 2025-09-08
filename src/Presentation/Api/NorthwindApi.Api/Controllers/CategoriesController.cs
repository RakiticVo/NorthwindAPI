using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Api.Handler;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.DTOs.Category;
using NorthwindApi.Application.Features.Category.Commands;
using NorthwindApi.Application.Features.Category.Queries;

namespace NorthwindApi.Api.Controllers;
    
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoriesController(Dispatcher dispatcher) : ControllerBase
{
    [HttpGet("get-all")]
    public async Task<IActionResult> GetCategories()
    {
        var data = await dispatcher.DispatchAsync(new GetCategories());
        return this.ReturnActionHandler(data);
    }
    
    [HttpGet("get/{id:int}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var data = await dispatcher.DispatchAsync(new GetCategoryById(id));
        return this.ReturnActionHandler(data);
    }
    
    [HttpPost("create-new")]
    public async Task<IActionResult> CreateCategory([FromBody]CreateCategoryRequest createCategoryRequest)
    {
        var data = await dispatcher.DispatchAsync(new CreateCategoryCommand(createCategoryRequest));
        return this.ReturnActionHandler(data);
    }
    
    [HttpPut("update")]
    public async Task<IActionResult> UpdateCategory([FromBody]UpdateCategoryRequest updateCategoryRequest)
    {
        var data = await dispatcher.DispatchAsync(new UpdateCategoryCommand(updateCategoryRequest));
        return this.ReturnActionHandler(data);
    }
    
    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var data = await dispatcher.DispatchAsync(new DeleteCategoryCommand(id));
        return this.ReturnActionHandler(data);
    }
}
