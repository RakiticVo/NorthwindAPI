namespace NorthwindApi.Application.DTOs.Region;

/// <summary>
/// Region Request
/// </summary>
public record CreateRegionRequest
{
    public string RegionDescription { get; init; } = null!;
}
public record UpdateRegionRequest
{
    public int Id { get; init; }
    public string RegionDescription { get; init; } = null!;
}

/// <summary>
/// Region Response
/// </summary>
public record RegionResponse
{
    public int Id { get; init; }
    public string RegionDescription { get; init; } = null!;
}