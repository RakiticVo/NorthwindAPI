using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindApi.Api.Entities;
using NorthwindApi.Application.DTOs.Product;
using NorthwindApi.Infrastructure;

namespace NorthwindApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(NorthwindContext context) : ControllerBase
{
    // GET: api/products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
    {
        var products = await context.Products
            .AsNoTracking()
            .Select(p => new ProductDto(
                p.ProductId,
                p.ProductName,
                p.SupplierId,
                p.CategoryId,
                p.QuantityPerUnit,
                p.UnitPrice,
                p.UnitsInStock,
                p.UnitsOnOrder,
                p.ReorderLevel,
                p.Discontinued
            ))
            .ToListAsync();

        return Ok(products);
    }

    // GET: api/products/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await context.Products
            .AsNoTracking()
            .Where(p => p.ProductId == id)
            .Select(p => new ProductDto(
                p.ProductId,
                p.ProductName,
                p.SupplierId,
                p.CategoryId,
                p.QuantityPerUnit,
                p.UnitPrice,
                p.UnitsInStock,
                p.UnitsOnOrder,
                p.ReorderLevel,
                p.Discontinued
            ))
            .FirstOrDefaultAsync();

        return product is null ? NotFound() : Ok(product);
    }

    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductRequest request)
    {
        var product = new Product
        {
            ProductName = request.ProductName,
            SupplierId = request.SupplierId,
            CategoryId = request.CategoryId,
            QuantityPerUnit = request.QuantityPerUnit,
            UnitPrice = request.UnitPrice,
            UnitsInStock = request.UnitsInStock,
            UnitsOnOrder = request.UnitsOnOrder,
            ReorderLevel = request.ReorderLevel,
            Discontinued = request.Discontinued
        };

        context.Products.Add(product);
        await context.SaveChangesAsync();

        var dto = new ProductDto(
            product.ProductId,
            product.ProductName,
            product.SupplierId,
            product.CategoryId,
            product.QuantityPerUnit,
            product.UnitPrice,
            product.UnitsInStock,
            product.UnitsOnOrder,
            product.ReorderLevel,
            product.Discontinued
        );

        return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, dto);
    }

    // PUT: api/products/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, UpdateProductRequest request)
    {
        var product = await context.Products.FindAsync(id);
        if (product is null) return NotFound();

        product.ProductName = request.ProductName;
        product.SupplierId = request.SupplierId;
        product.CategoryId = request.CategoryId;
        product.QuantityPerUnit = request.QuantityPerUnit;
        product.UnitPrice = request.UnitPrice;
        product.UnitsInStock = request.UnitsInStock;
        product.UnitsOnOrder = request.UnitsOnOrder;
        product.ReorderLevel = request.ReorderLevel;
        product.Discontinued = request.Discontinued;

        await context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/products/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product is null) return NotFound();

        context.Products.Remove(product);
        await context.SaveChangesAsync();

        return NoContent();
    }
}
