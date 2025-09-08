namespace NorthwindApi.Application.DTOs.Customer;

/// <summary>
/// Customer Request
/// </summary>
public record CreateCustomerRequest
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
}

public record UpdateCustomerRequest
{
    public string Id { get; init; } = null!;
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
}

/// <summary>
/// Customer Response
/// </summary>
public record CustomerResponse
{
    public string Id { get; init; } = null!;
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
}