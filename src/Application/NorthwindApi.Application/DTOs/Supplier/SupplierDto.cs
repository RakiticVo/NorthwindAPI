namespace NorthwindApi.Application.DTOs.Supplier;

/// <summary>
/// Product Requ
/// </summary>
public record CreateSupplierRequest
{
    public string CompanyName { get; init; } = null!;
    public string? ContactName { get; init; }
    public string? ContactTitle { get; init; }
    public string? Address { get; init; }
    public string? City { get; init; }
    public string? Region { get; init; }
    public string? PostalCode { get; init; }
    public string? Country { get; init; }
    public string? Phone { get; init; }
    public string? Fax { get; init; }
    public string? HomePage { get; init; }
}

public record UpdateSupplierRequest
{
    public int Id { get; init; }
    public string CompanyName { get; init; } = null!;
    public string? ContactName { get; init; }
    public string? ContactTitle { get; init; }
    public string? Address { get; init; }
    public string? City { get; init; }
    public string? Region { get; init; }
    public string? PostalCode { get; init; }
    public string? Country { get; init; }
    public string? Phone { get; init; }
    public string? Fax { get; init; }
    public string? HomePage { get; init; }
}

/// <summary>
/// Product Response
/// </summary>
public record SupplierResponse
{
    public int Id { get; init; }
    public string CompanyName { get; init; } = null!;
    public string? ContactName { get; init; }
    public string? ContactTitle { get; init; }
    public string? Address { get; init; }
    public string? City { get; init; }
    public string? Region { get; init; }
    public string? PostalCode { get; init; }
    public string? Country { get; init; }
    public string? Phone { get; init; }
    public string? Fax { get; init; }
    public string? HomePage { get; init; }
}