namespace NorthwindApi.Application.DTOs.Product;

/// <summary>
/// Product Requests
/// </summary>
public record CreateProductRequest
{
    public string ProductName { get; init; } = null!;
    public int? SupplierId { get; init; }
    public int? CategoryId { get; init; }
    public string? QuantityPerUnit { get; init; } = null!;
    public decimal? UnitPrice { get; init; }
    public short? UnitsInStock { get; init; }
    public short? UnitsOnOrder { get; init; }
    public short? ReorderLevel { get; init; }
    public bool Discontinued { get; init; }
};

public record UpdateProductRequest
{
    public int Id { get; init; }
    public string ProductName { get; init; } = null!;
    public int? SupplierId { get; init; }
    public int? CategoryId { get; init; }
    public string? QuantityPerUnit { get; init; }
    public decimal? UnitPrice { get; init; }
    public short? UnitsInStock { get; init; }
    public short? UnitsOnOrder { get; init; }
    public short? ReorderLevel { get; init; }
    public bool Discontinued { get; init; }
}
    
/// <summary>
/// Product Response
/// </summary>
public record ProductResponse
{
    public int Id { get; init; }
    public string ProductName { get; init; } = null!;
    public int? SupplierId { get; init; }
    public string? CompanyName { get; init; }
    public int? CategoryId { get; init; }
    public string? CategoryName { get; init; }
    public string? QuantityPerUnit { get; init; }
    public decimal? UnitPrice { get; init; }
    public short? UnitsInStock { get; init; }
    public short? UnitsOnOrder { get; init; }
    public short? ReorderLevel { get; init; }
    public bool Discontinued { get; init; }
}
