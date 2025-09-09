using System.ComponentModel;

namespace NorthwindApi.Application.DTOs.Order;

/// <summary>
/// Order Request
/// </summary>
public record GetOrderByPageRequest
{
    [DefaultValue(1)]
    public int PageNumber { get; init; }
    [DefaultValue(100)]
    public int PageSize { get; init; }
}
public record CreateOrderRequest
{
    public string? CustomerId { get; init; }
    public int? EmployeeId { get; init; }
    public DateTime? OrderDate { get; init; }
    public DateTime? RequiredDate { get; init; }
    public DateTime? ShippedDate { get; init; }
    public int? ShipVia { get; init; }
    public decimal? Freight { get; init; }
    public string? ShipName { get; init; }
    public string? ShipAddress { get; init; }
    public string? ShipCity { get; init; }
    public string? ShipRegion { get; init; }
    public string? ShipPostalCode { get; init; }
    public string? ShipCountry { get; init; }
}

public record UpdateOrderRequest
{
    public int Id { get; init; }
    public string? CustomerId { get; init; }
    public int? EmployeeId { get; init; }
    public DateTime? OrderDate { get; init; }
    public DateTime? RequiredDate { get; init; }
    public DateTime? ShippedDate { get; init; }
    public int? ShipVia { get; init; }
    public decimal? Freight { get; init; }
    public string? ShipName { get; init; }
    public string? ShipAddress { get; init; }
    public string? ShipCity { get; init; }
    public string? ShipRegion { get; init; }
    public string? ShipPostalCode { get; init; }
    public string? ShipCountry { get; init; }
}

/// <summary>
/// Order Response
/// </summary>
public record OrderResponse
{
    public int Id { get; init; }
    public string? CustomerId { get; init; }
    public int? EmployeeId { get; init; }
    public DateTime? OrderDate { get; init; }
    public DateTime? RequiredDate { get; init; }
    public DateTime? ShippedDate { get; init; }
    public int? ShipVia { get; init; }
    public decimal? Freight { get; init; }
    public string? ShipName { get; init; }
    public string? ShipAddress { get; init; }
    public string? ShipCity { get; init; }
    public string? ShipRegion { get; init; }
    public string? ShipPostalCode { get; init; }
    public string? ShipCountry { get; init; }
};