using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.DTOs.Product;
using NorthwindApi.Application.Features.Product.Commands;
using NorthwindApi.Application.Features.Product.Queries;

namespace NorthwindApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(Dispatcher dispatcher) : ControllerBase
{
    // GET: api/products
    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetProducts()
    {
        var data = await dispatcher.DispatchAsync(new GetProducts());
        return Ok(data.Result);
    }

    // GET: api/products/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse>> GetProduct(int id)
    {
        var data = await dispatcher.DispatchAsync(new GetProductById(id));
        if(data.Result is not ProductDto product)
        {
            return NotFound(data.Message);
        }
        return Ok(data.Result);
    }

    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CreateProduct(CreateProductRequest request)
    {
        var data = await dispatcher.DispatchAsync(new CreateProductCommand(request));
        var product = (ProductDto)data.Result;
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, data.Result);
    }

    // PUT: api/products/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, UpdateProductRequest request)
    {
        var data = await dispatcher.DispatchAsync(new UpdateProductCommand(id, request));
        if (data.Result is not ProductDto)
        {
            return NotFound(data.Message);
        }
        return Ok(data.Result);
    }

    // DELETE: api/products/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
       var data = await dispatcher.DispatchAsync(new DeleteProductCommand(id));
       if (data.StatusCode == 200)
       {
           return Ok(data.Message);
       }
       return NotFound(data.Message);
    }
}
