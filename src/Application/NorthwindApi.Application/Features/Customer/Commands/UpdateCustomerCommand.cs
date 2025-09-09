using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Customer;

namespace NorthwindApi.Application.Features.Customer.Commands;

public record UpdateCustomerCommand(UpdateCustomerRequest UpdateCustomerRequest) : ICommand<ApiResponse>;

internal class UpdateCustomerCommandHandler(
    ICrudService<Domain.Entities.Customer, string> crudService,
    IMapper mapper,
    IUnitOfWork unitOfWork
) : ICommandHandler<UpdateCustomerCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(UpdateCustomerCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var existingCustomer = await crudService.GetByIdAsync(command.UpdateCustomerRequest.Id);
            if (existingCustomer == null) return new ApiResponse(StatusCodes.Status404NotFound, "Customer not found!!!");
            
            mapper.Map(command.UpdateCustomerRequest, existingCustomer);
            await crudService.UpdateAsync(existingCustomer, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            var customerDto = mapper.Map<CustomerResponse>(existingCustomer);
            return new ApiResponse(StatusCodes.Status200OK, "Customer updated successfully!!!" , customerDto);
        }
    }
}