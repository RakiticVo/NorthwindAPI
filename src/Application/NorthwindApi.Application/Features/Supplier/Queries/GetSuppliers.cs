using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Queries;
using NorthwindApi.Application.DTOs.Supplier;

namespace NorthwindApi.Application.Features.Supplier.Queries;

public record GetSuppliers : IQuery<ApiResponse>;

internal class GetSuppliersHandler(
    ICrudService<Domain.Entities.Supplier, int> crudService,
    IMapper mapper
) : IQueryHandler<GetSuppliers, ApiResponse>
{

    public async Task<ApiResponse?> HandleAsync(GetSuppliers query, CancellationToken cancellationToken = default)
    {
        var response = await crudService.GetAsync();
        var suppliers = mapper.Map<List<SupplierResponse>>(response);
        return new ApiResponse(StatusCodes.Status200OK, "Get Suppliers successfully!!!", suppliers);
    }
}