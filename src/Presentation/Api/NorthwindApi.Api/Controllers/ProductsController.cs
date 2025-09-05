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
    // GET: api/products
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var data = await dispatcher.DispatchAsync(new GetProducts());
        return this.ReturnActionHandler(data);
    }

    // GET: api/products/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var data = await dispatcher.DispatchAsync(new GetProductById(id));
        return this.ReturnActionHandler(data);
    }

    // POST: api/products
    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductRequest request)
    {
        var data = await dispatcher.DispatchAsync(new CreateProductCommand(request));
        return this.ReturnActionHandler(data);
    }

    // PUT: api/products/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, UpdateProductRequest request)
    {
        var data = await dispatcher.DispatchAsync(new UpdateProductCommand(id, request));
        return this.ReturnActionHandler(data);
    }

    // DELETE: api/products/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
       var data = await dispatcher.DispatchAsync(new DeleteProductCommand(id));
       return this.ReturnActionHandler(data);
    }
}
