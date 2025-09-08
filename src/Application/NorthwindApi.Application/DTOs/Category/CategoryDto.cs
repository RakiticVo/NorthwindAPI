namespace NorthwindApi.Application.DTOs.Category;

/// <summary>
/// Category Response
/// </summary>
public record CreateCategoryRequest
{
    public string CategoryName { get; init; } = null!;
    public string? Description { get; init; }
}

public record UpdateCategoryRequest
{
    public int CategoryId { get; init; }
    public string CategoryName { get; init; } = null!;
    public string? Description { get; init; }
}

/// <summary>
/// Category Response
/// </summary>
public record CategoryResponse
{
    public int CategoryId { get; init; }
    public string CategoryName { get; init; } = null!;
    public string? Description { get; init; }
}