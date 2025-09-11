namespace NorthwindApi.Application.DTOs.Shipper;

/// <summary>
/// Shipper Request
/// </summary>
public record CreateShipperRequest
{
    public string CompanyName { get; set; } = null!;
    public string? Phone { get; set; }
}

public record UpdateShipperRequest
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = null!;
    public string? Phone { get; set; }
}

/// <summary>
/// Shipper Response
/// </summary>
public record ShipperResponse
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = null!;
    public string? Phone { get; set; }
}