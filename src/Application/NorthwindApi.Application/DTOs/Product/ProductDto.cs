namespace NorthwindApi.Application.DTOs.Product;

public record ProductDto(
    int Id,
    string ProductName,
    int? SupplierId,
    int? CategoryId,
    string? QuantityPerUnit,
    decimal? UnitPrice,
    short? UnitsInStock,
    short? UnitsOnOrder,
    short? ReorderLevel,
    bool Discontinued);

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
    string ProductName,
    int? SupplierId,
    int? CategoryId,
    string? QuantityPerUnit,
    decimal? UnitPrice,
    short? UnitsInStock,
    short? UnitsOnOrder,
    short? ReorderLevel,
    bool Discontinued);