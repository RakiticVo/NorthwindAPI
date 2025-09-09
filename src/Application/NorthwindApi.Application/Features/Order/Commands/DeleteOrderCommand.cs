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
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var existingOrder = await crudService.GetByIdAsync(command.OrderId);
            if (existingOrder == null) return new ApiResponse(StatusCodes.Status404NotFound, "Order not found!!!");
            
            await crudService.DeleteAsync(existingOrder, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return new ApiResponse(StatusCodes.Status200OK, "Order deleted successfully!!!");
        }
    }
}