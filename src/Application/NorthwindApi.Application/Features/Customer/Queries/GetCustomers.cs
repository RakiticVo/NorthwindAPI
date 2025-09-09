using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Queries;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Customer;

namespace NorthwindApi.Application.Features.Customer.Queries;

public record GetCustomers() : IQuery<ApiResponse>;

internal class GetCustomersHandler(
    ICrudService<Domain.Entities.Customer, string> crudService,
    IMapper mapper    
) : IQueryHandler<GetCustomers, ApiResponse>
{
    public async Task<ApiResponse?> HandleAsync(GetCustomers query, CancellationToken cancellationToken = default)
    {
        var customers = await crudService.GetAsync();
        return customers.Count == 0
            ? new ApiResponse(StatusCodes.Status404NotFound, "No Customers found!!!")
            : new ApiResponse(StatusCodes.Status200OK, "Get Customers successfully!!!", mapper.Map<List<CustomerResponse>>(customers));
    }
}