using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Queries;
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
        var response = await crudService.GetAsync();
        var categories = mapper.Map<List<CategoryDto>>(response);
        return new ApiResponse(StatusCodes.Status200OK, "Get Categories successfully!!!", categories);
    }
}