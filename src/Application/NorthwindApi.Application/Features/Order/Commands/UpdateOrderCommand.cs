using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Order;

namespace NorthwindApi.Application.Features.Order.Commands;

public record UpdateOrderCommand(UpdateOrderRequest UpdateOrderRequest) : ICommand<ApiResponse>;

internal class UpdateOrderCommandHandler(
    ICrudService<Domain.Entities.Order, int> crudService,
    IMapper mapper,
    IUnitOfWork unitOfWork    
) : ICommandHandler<UpdateOrderCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(UpdateOrderCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var existingOrder = await crudService.GetByIdAsync(command.UpdateOrderRequest.Id);
            if (existingOrder == null) return new ApiResponse(StatusCodes.Status404NotFound, "Order not found!!!");
            
            mapper.Map(command.UpdateOrderRequest, existingOrder);
            await crudService.UpdateAsync(existingOrder, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            var orderDto = mapper.Map<OrderResponse>(existingOrder);
            return new ApiResponse(StatusCodes.Status200OK, "Order updated successfully!!!", orderDto);
        }
    }
}