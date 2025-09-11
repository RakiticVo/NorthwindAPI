using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Queries;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs;
using NorthwindApi.Application.DTOs.Order;

namespace NorthwindApi.Application.Features.Order.Queries;

public record GetOrdersByPage(GetOrderByPageRequest GetOrderByPageRequest) : IQuery<ApiResponse>;

internal class GetOrdersByPageHandler(
    IRepository<Domain.Entities.Order, int> repository,
    IMapper mapper
) : IQueryHandler<GetOrdersByPage, ApiResponse>
{
    public async Task<ApiResponse?> HandleAsync(GetOrdersByPage query, CancellationToken cancellationToken = default)
    {
        var ordersQuery = repository.GetQueryableSet();
        var totalCount = await ordersQuery.CountAsync(cancellationToken: cancellationToken);
        
        var orders = await ordersQuery
            .Skip((query.GetOrderByPageRequest.PageNumber - 1) * query.GetOrderByPageRequest.PageSize)
            .Take(query.GetOrderByPageRequest.PageSize)
            .ToListAsync(cancellationToken);
        
        if (orders.Count == 0) return new ApiResponse(StatusCodes.Status404NotFound, "No Orders found!!!");

        var orderDto = mapper.Map<List<OrderResponse>>(orders);

        var pagedResponse = new PagedResponse<OrderResponse>(
            orderDto,
            query.GetOrderByPageRequest.PageNumber,
            query.GetOrderByPageRequest.PageSize,
            totalCount
        );
        
        return new ApiResponse(StatusCodes.Status200OK, "Get Orders successfully!!!", pagedResponse);
    }
}