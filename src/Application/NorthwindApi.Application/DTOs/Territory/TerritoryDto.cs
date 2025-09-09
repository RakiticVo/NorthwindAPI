namespace NorthwindApi.Application.DTOs.Territory;

/// <summary>
/// Territory Request
/// </summary>
public record CreateTerritoryRequest
{
    public string Id { get; init; } = null!;
    public string TerritoryDescription { get; init; } = null!;
    public int RegionId { get; init; }
}

public record UpdateTerritoryRequest
{
    public string Id { get; init; } = null!;
    public string TerritoryDescription { get; init; } = null!;
    public int RegionId { get; init; }
}

/// <summary>
/// Territory Response
/// </summary>
public record TerritoryResponse
{
    public string  Id { get; init; } = null!;
    public string TerritoryDescription { get; init; } = null!;
    public int RegionId { get; init; }
}