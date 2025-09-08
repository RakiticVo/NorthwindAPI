using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Api.Handler;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.DTOs.Product;
using NorthwindApi.Application.Features.Product.Commands;
using NorthwindApi.Application.Features.Product.Queries;

namespace NorthwindApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController(Dispatcher dispatcher) : ControllerBase
{
    [HttpGet("get-all")]
    public async Task<IActionResult> GetProducts()
    {
        var data = await dispatcher.DispatchAsync(new GetProducts());
        return this.ReturnActionHandler(data);
    }
    
    [HttpGet("get/{id:int}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var data = await dispatcher.DispatchAsync(new GetProductById(id));
        return this.ReturnActionHandler(data);
    }
    
    [HttpPost("create-new")]
    public async Task<IActionResult> CreateProduct([FromBody]CreateProductRequest request)
    {
        var data = await dispatcher.DispatchAsync(new CreateProductCommand(request));
        return this.ReturnActionHandler(data);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateProduct([FromBody]UpdateProductRequest request)
    {
        var data = await dispatcher.DispatchAsync(new UpdateProductCommand(request));
        return this.ReturnActionHandler(data);
    }
    
    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
       var data = await dispatcher.DispatchAsync(new DeleteProductCommand(id));
       return this.ReturnActionHandler(data);
    }
}
