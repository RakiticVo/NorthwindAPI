using System.Data;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;

namespace NorthwindApi.Application.Features.Customer.Commands;

public record DeleteCustomerCommand(string CustomerId) : ICommand<ApiResponse>;

internal class DeleteCustomerCommandHandler(
    ICrudService<Domain.Entities.Customer, string> crudService,
    IUnitOfWork unitOfWork
) : ICommandHandler<DeleteCustomerCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(DeleteCustomerCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var existingCustomer = await crudService.GetByIdAsync(command.CustomerId);
            if (existingCustomer == null) return new ApiResponse(StatusCodes.Status404NotFound, "Customer not found!!!");
            
            await crudService.DeleteAsync(existingCustomer, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return new ApiResponse(StatusCodes.Status200OK, "Delete Customer successfully!!!");
        }
    }
}