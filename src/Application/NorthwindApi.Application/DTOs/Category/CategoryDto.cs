namespace NorthwindApi.Application.DTOs.Category;

/// <summary>
/// Category Response
/// </summary>
public record CreateCategoryRequest(string CategoryName, string? Description);

public record UpdateCategoryRequest(int CategoryId, string CategoryName, string? Description);

/// <summary>
/// Category Response
/// </summary>
public record CategoryDto(
    int Id,
    string CategoryName,
    string? Description);