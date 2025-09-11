using System.Data;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.Common.Response;

namespace NorthwindApi.Application.Features.Customer.Commands;

public record DeleteCustomerCommand(string CustomerId) : ICommand<ApiResponse>;

internal class DeleteCustomerCommandHandler(
    ICrudService<Domain.Entities.Customer, string> crudService,
    IUnitOfWork unitOfWork
) : ICommandHandler<DeleteCustomerCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(DeleteCustomerCommand command, CancellationToken cancellationToken = default)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async token =>
        {
            var existingCustomer = await crudService.GetByIdAsync(command.CustomerId);
            if (existingCustomer == null) return new ApiResponse(StatusCodes.Status404NotFound, "Customer not found!!!");
            
            await crudService.DeleteAsync(existingCustomer, token);
            return new ApiResponse(StatusCodes.Status200OK, "Customer deleted successfully!!!");
        }, cancellationToken: cancellationToken);
    }
}