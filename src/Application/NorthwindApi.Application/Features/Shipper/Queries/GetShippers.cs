using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Queries;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Shipper;

namespace NorthwindApi.Application.Features.Shipper.Queries;

public record GetShippers : IQuery<ApiResponse>;

internal class GetShippersHandler(
    ICrudService<Domain.Entities.Shipper, int> crudService,
    IMapper mapper
) : IQueryHandler<GetShippers, ApiResponse>
{
    public async Task<ApiResponse?> HandleAsync(GetShippers query, CancellationToken cancellationToken = default)
    {
        var shippers = await crudService.GetAsync();
        return shippers.Count == 0
            ? new ApiResponse(StatusCodes.Status404NotFound, "No Shippers found!!!")
            : new ApiResponse(StatusCodes.Status200OK, "Get Shippers successfully!!!", mapper.Map<List<ShipperResponse>>(shippers));
    }
}