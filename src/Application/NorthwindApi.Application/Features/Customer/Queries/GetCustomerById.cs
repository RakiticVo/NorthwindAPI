using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Queries;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Customer;

namespace NorthwindApi.Application.Features.Customer.Queries;

public record GetCustomerById(string CustomerId) : IQuery<ApiResponse>;

internal class GetCustomerByIdHandler(
    ICrudService<Domain.Entities.Customer, string> crudService,
    IMapper mapper  
) : IQueryHandler<GetCustomerById, ApiResponse>
{
    public async Task<ApiResponse?> HandleAsync(GetCustomerById query, CancellationToken cancellationToken = default)
    {
        var customer = await crudService.GetByIdAsync(query.CustomerId);
        return customer == null
        ? new ApiResponse(StatusCodes.Status404NotFound, "Customer not found!!!")
        : new ApiResponse(StatusCodes.Status200OK, "Get Customer by Id successfully!!!", mapper.Map<CustomerResponse>(customer));
    }
}