using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Order;

namespace NorthwindApi.Application.Features.Order.Commands;

public record CreateOrderCommand(CreateOrderRequest CreateOrderRequest) : ICommand<ApiResponse>;

internal class CreateOrderCommandHandler(
    ICrudService<Domain.Entities.Order, int> crudService,
    IRepository<Domain.Entities.Order, int> repository,
    IMapper mapper,
    IUnitOfWork unitOfWork
) : ICommandHandler<CreateOrderCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(CreateOrderCommand command, CancellationToken cancellationToken = default)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async token =>
        {
            var order = mapper.Map<Domain.Entities.Order>(command.CreateOrderRequest);
            var orders = await crudService.GetAsync();
            if (orders.Any(orderItem => orderItem.Equals(order)))
                return new ApiResponse(StatusCodes.Status409Conflict, "Order already exists!!!");
            
            await crudService.AddAsync(order, token);
            var newOrder = await repository
                .FirstOrDefaultAsync(repository.GetQueryableSet()
                    .Where(x => x.Equals(order)));
            var orderDto = mapper.Map<OrderResponse>(newOrder);
            return new ApiResponse(StatusCodes.Status201Created, "Order created successfully!!", orderDto);
        }, cancellationToken: cancellationToken);
    }
}