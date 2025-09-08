using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Queries;
using NorthwindApi.Application.DTOs.Supplier;

namespace NorthwindApi.Application.Features.Supplier.Queries;

public record GetSupplierById(int SupplierId) : IQuery<ApiResponse>;

internal class GetSupplierByIdHandler(
    ICrudService<Domain.Entities.Supplier, int> crudService,
    IMapper mapper
) : IQueryHandler<GetSupplierById, ApiResponse>
{
    public async Task<ApiResponse?> HandleAsync(GetSupplierById query, CancellationToken cancellationToken = default)
    {
        var supplier = await crudService.GetByIdAsync(query.SupplierId);
        if (supplier == null) return new ApiResponse(StatusCodes.Status404NotFound, "Supplier not found!!!");
        var supplierDto = mapper.Map<SupplierResponse>(supplier);
        return new ApiResponse(StatusCodes.Status200OK, "Get Suppliers successfully!!!", supplierDto);
    }
}