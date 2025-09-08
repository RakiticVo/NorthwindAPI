namespace NorthwindApi.Application.DTOs.Product;

/// <summary>
/// Product Requests
/// </summary>
public record CreateProductRequest(
    string ProductName,
    int? SupplierId,
    int? CategoryId,
    string? QuantityPerUnit,
    decimal? UnitPrice,
    short? UnitsInStock,
    short? UnitsOnOrder,
    short? ReorderLevel,
    bool Discontinued);

public record UpdateProductRequest(
    int ProductId,
    string ProductName,
    int? SupplierId,
    int? CategoryId,
    string? QuantityPerUnit,
    decimal? UnitPrice,
    short? UnitsInStock,
    short? UnitsOnOrder,
    short? ReorderLevel,
    bool Discontinued);
    
/// <summary>
/// Product Response
/// </summary>
public record ProductDto
{
    public int Id { get; init; }
    public string ProductName { get; init; } = "";
    public int? SupplierId { get; init; }
    public int? CategoryId { get; init; }
    public string? CategoryName { get; init; }
    public string? QuantityPerUnit { get; init; }
    public decimal? UnitPrice { get; init; }
    public short? UnitsInStock { get; init; }
    public short? UnitsOnOrder { get; init; }
    public short? ReorderLevel { get; init; }
    public bool Discontinued { get; init; }
}
