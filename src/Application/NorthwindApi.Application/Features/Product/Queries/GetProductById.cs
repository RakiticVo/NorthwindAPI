using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Queries;
using NorthwindApi.Application.DTOs.Product;

namespace NorthwindApi.Application.Features.Product.Queries;

public record GetProductById(int ProductId) : IQuery<ApiResponse>
{
    public int ProductId { get; set; } = ProductId;
}

internal class GetProductByIdHandler(
    ICrudService<Domain.Entities.Product, int> crudService,
    IMapper mapper
) : IQueryHandler<GetProductById, ApiResponse>
{
    public async Task<ApiResponse?> HandleAsync(GetProductById query, CancellationToken cancellationToken = default)
    {
        var response = await crudService.GetByIdAsync(query.ProductId);
        return response == null 
            ? new ApiResponse(StatusCodes.Status404NotFound, "Product not found") 
            : new ApiResponse(StatusCodes.Status200OK, "Get product by id successfully", mapper.Map<ProductDto>(response));
    }
}