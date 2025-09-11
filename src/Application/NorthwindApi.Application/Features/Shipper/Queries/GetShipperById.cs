using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Queries;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Shipper;

namespace NorthwindApi.Application.Features.Shipper.Queries;

public record GetShipperById(int ShipperId) : IQuery<ApiResponse>;

internal class GetShipperByIdHandler(
    ICrudService<Domain.Entities.Shipper, int> crudService,
    IMapper mapper
) : IQueryHandler<GetShipperById, ApiResponse>
{
    public async Task<ApiResponse?> HandleAsync(GetShipperById query, CancellationToken cancellationToken = default)
    {
        var shipper = await crudService.GetByIdAsync(query.ShipperId);
        return shipper is null
            ? new ApiResponse(StatusCodes.Status404NotFound, "Shipper not found!!!")
            : new ApiResponse(StatusCodes.Status200OK, "Get Shipper by Id successfully!!!", mapper.Map<ShipperResponse>(shipper));
    }
}