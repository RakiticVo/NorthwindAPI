using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Queries;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Order;

namespace NorthwindApi.Application.Features.Order.Queries;

public record GetOrderById(int OrderId) : IQuery<ApiResponse>;

internal class GetOrderByIdHandler(
    ICrudService<Domain.Entities.Order, int> crudService,
    IMapper mapper
) : IQueryHandler<GetOrderById, ApiResponse>
{
    public async Task<ApiResponse?> HandleAsync(GetOrderById query, CancellationToken cancellationToken = default)
    {
        var order = await crudService.GetByIdAsync(query.OrderId);
        return order == null
            ? new ApiResponse(StatusCodes.Status404NotFound, "Order not found!!!")
            : new ApiResponse(StatusCodes.Status200OK, "Get Order by Id successfully!!!", mapper.Map<OrderResponse>(order));
    }
}