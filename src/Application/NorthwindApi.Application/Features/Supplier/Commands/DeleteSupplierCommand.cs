using System.Data;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.Common.Response;

namespace NorthwindApi.Application.Features.Supplier.Commands;

public record DeleteSupplierCommand(int SupplierId) : ICommand<ApiResponse>;

internal class DeleteSupplierCommandHandler(
    ICrudService<Domain.Entities.Supplier, int> crudService,
    IUnitOfWork unitOfWork
) : ICommandHandler<DeleteSupplierCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(DeleteSupplierCommand command, CancellationToken cancellationToken = default)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async token =>
        {
            var existingSupplier = await crudService.GetByIdAsync(command.SupplierId);
            if (existingSupplier == null) 
                return new ApiResponse(StatusCodes.Status404NotFound, "Supplier not found!!!");

            await crudService.DeleteAsync(existingSupplier, token);
            return new ApiResponse(StatusCodes.Status200OK, "Supplier deleted successfully!!!"); 
        }, cancellationToken: cancellationToken);
    }
}