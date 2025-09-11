using System.Data;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.Common.Response;

namespace NorthwindApi.Application.Features.Order.Commands;

public record DeleteOrderCommand(int OrderId) : ICommand<ApiResponse>;

internal class DeleteOrderCommandHandler(
    ICrudService<Domain.Entities.Order, int> crudService,
    IUnitOfWork unitOfWork
) : ICommandHandler<DeleteOrderCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(DeleteOrderCommand command, CancellationToken cancellationToken = default)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async token =>
        {
            var existingOrder = await crudService.GetByIdAsync(command.OrderId);
            if (existingOrder == null) 
                return new ApiResponse(StatusCodes.Status404NotFound, "Order not found!!!");

            await crudService.DeleteAsync(existingOrder, token);
            return new ApiResponse(StatusCodes.Status200OK, "Order deleted successfully!!!");
        }, cancellationToken: cancellationToken);
    }
}