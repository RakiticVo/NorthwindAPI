using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Queries;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Category;

namespace NorthwindApi.Application.Features.Category.Queries;

public record GetCategoryById(int CategoryId) : IQuery<ApiResponse>;

internal class GetCategoriesByIdHandler(
    ICrudService<Domain.Entities.Category, int> crudService,
    IMapper mapper    
) : IQueryHandler<GetCategoryById, ApiResponse>
{
    public async Task<ApiResponse?> HandleAsync(GetCategoryById query, CancellationToken cancellationToken = default)
    {
        var category = await crudService.GetByIdAsync(query.CategoryId);
        return category == null
        ? new ApiResponse(StatusCodes.Status404NotFound, "Category not found!!!")
        : new ApiResponse(StatusCodes.Status200OK, "Get Category by Id successfully!!!", mapper.Map<CategoryResponse>(category));
    }
}