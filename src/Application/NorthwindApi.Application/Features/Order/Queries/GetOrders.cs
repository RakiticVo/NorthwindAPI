using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Queries;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Order;

namespace NorthwindApi.Application.Features.Order.Queries;

public record GetOrders : IQuery<ApiResponse>;

internal class GetOrdersHandler(
    ICrudService<Domain.Entities.Order, int> crudService,
    IMapper mapper
) : IQueryHandler<GetOrders, ApiResponse>
{
    public async Task<ApiResponse?> HandleAsync(GetOrders query, CancellationToken cancellationToken = default)
    {
        var orders = await crudService.GetAsync();
        return orders.Count == 0
            ? new ApiResponse(StatusCodes.Status404NotFound, "No Orders found!!!")
            : new ApiResponse(StatusCodes.Status200OK, "Get Orders successfully!!!", mapper.Map<List<OrderResponse>>(orders));
    }
}