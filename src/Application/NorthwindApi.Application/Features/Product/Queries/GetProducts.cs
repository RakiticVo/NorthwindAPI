using AutoMapper;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Queries;
using NorthwindApi.Application.DTOs.Product;
using NorthwindApi.Infrastructure.Cache;

namespace NorthwindApi.Application.Features.Product.Queries;

public record GetProducts : IQuery<ApiResponse>;

internal class GetProductsHandler(
    ICrudService<Domain.Entities.Product, int> crudService,
    IMapper mapper,
    BusinessCacheService cacheService
) : IQueryHandler<GetProducts, ApiResponse>
{
    public async Task<ApiResponse?> HandleAsync(GetProducts query, CancellationToken cancellationToken = default)
    {
        return await cacheService.GetOrSetAsync(
            CacheKeys.Products,
            async () =>
            {
                var response = await crudService.GetAsync();
                var products = mapper.Map<List<ProductDto>>(response);
                return new ApiResponse(200, "Get product successfully", products);
            },
            TimeSpan.FromMinutes(5),
            cancellationToken);
    }
}