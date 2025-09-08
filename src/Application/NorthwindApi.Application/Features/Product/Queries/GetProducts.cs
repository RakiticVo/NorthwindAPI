using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Queries;
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
        var response = await crudService.GetAsync();
        var products = mapper.Map<List<ProductDto>>(response);
        return new ApiResponse(StatusCodes.Status200OK, "Get product successfully", products);
    }
}