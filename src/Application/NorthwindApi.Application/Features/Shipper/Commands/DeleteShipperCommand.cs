using System.Data;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.Common.Response;

namespace NorthwindApi.Application.Features.Shipper.Commands;

public record DeleteShipperCommand(int ShipperId) : ICommand<ApiResponse>;

internal class DeleteShipperCommandHandler(
    ICrudService<Domain.Entities.Shipper, int> crudService,
    IUnitOfWork unitOfWork
) : ICommandHandler<DeleteShipperCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(DeleteShipperCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var existingShipper = await crudService.GetByIdAsync(command.ShipperId);
            if (existingShipper == null) return new ApiResponse(StatusCodes.Status404NotFound, "Shipper not found!!!");
        
            await crudService.DeleteAsync(existingShipper, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return new ApiResponse(StatusCodes.Status200OK, "Shipper deleted successfully!!!");
        }
    }
}