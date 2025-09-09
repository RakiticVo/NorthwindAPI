using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Queries;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Product;

namespace NorthwindApi.Application.Features.Product.Queries;

public record GetProducts : IQuery<ApiResponse>;

internal class GetProductsHandler(
    ICrudService<Domain.Entities.Product, int> crudService,
    IMapper mapper
) : IQueryHandler<GetProducts, ApiResponse>
{
    public async Task<ApiResponse?> HandleAsync(GetProducts query, CancellationToken cancellationToken = default)
    {
        var products = await crudService.GetAsync();
        return products.Count == 0
        ? new ApiResponse(StatusCodes.Status404NotFound, "No Products found!!!")
        : new ApiResponse(StatusCodes.Status200OK, "Get products successfully!!!", mapper.Map<List<ProductResponse>>(products));
    }
}