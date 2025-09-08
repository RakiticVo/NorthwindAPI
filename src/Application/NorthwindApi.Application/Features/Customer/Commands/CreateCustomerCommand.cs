using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.DTOs.Customer;

namespace NorthwindApi.Application.Features.Customer.Commands;

public record CreateCustomerCommand(CreateCustomerRequest CreateCustomerRequest) : ICommand<ApiResponse>;

internal class CreateCustomerCommandHandler(
    ICrudService<Domain.Entities.Customer, string> crudService,
    IRepository<Domain.Entities.Customer, string> repository,
    IMapper mapper,
    IUnitOfWork unitOfWork
) : ICommandHandler<CreateCustomerCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(CreateCustomerCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var existingCustomer = mapper.Map<Domain.Entities.Customer>(command.CreateCustomerRequest);
            var customers = await crudService.GetAsync();
            if (customers.Any(customerItem => customerItem.CompanyName == existingCustomer.CompanyName)) 
                return new ApiResponse(StatusCodes.Status409Conflict, "Customer already exists");
            await crudService.AddAsync(existingCustomer, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            var newCustomer = await repository
                .FirstOrDefaultAsync(repository.GetQueryableSet()
                    .Where(x => x.CompanyName == existingCustomer.CompanyName));
            var customerDto = mapper.Map<CustomerResponse>(newCustomer);
            return new ApiResponse(StatusCodes.Status200OK, "Customer created successfully!!!", customerDto);
        }
    }
}