using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Queries;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Category;

namespace NorthwindApi.Application.Features.Category.Queries;

public record GetCategories : IQuery<ApiResponse>;

internal class GetCategoriesHandler(
    ICrudService<Domain.Entities.Category, int> crudService,
    IMapper mapper    
) : IQueryHandler<GetCategories, ApiResponse>
{
    public async Task<ApiResponse?> HandleAsync(GetCategories query, CancellationToken cancellationToken = default)
    {
        var categories = await crudService.GetAsync();
        return categories.Count == 0
            ? new ApiResponse(StatusCodes.Status404NotFound, "No Categories found!!!")
            : new ApiResponse(StatusCodes.Status200OK, "Get Categories successfully!!!", mapper.Map<List<CategoryResponse>>(categories));
    }
}