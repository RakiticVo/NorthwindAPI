using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Queries;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Region;

namespace NorthwindApi.Application.Features.Region.Queries;

public record GetRegionById(int RegionId) : IQuery<ApiResponse>;

internal class GetRegionByIdHandler(
    ICrudService<Domain.Entities.Region, int> crudService,
    IMapper mapper
) : IQueryHandler<GetRegionById, ApiResponse>
{
    public async Task<ApiResponse?> HandleAsync(GetRegionById query, CancellationToken cancellationToken = default)
    {
        var region = await crudService.GetByIdAsync(query.RegionId);
        return region == null
            ? new ApiResponse(StatusCodes.Status404NotFound, "Region not found!!!")
            : new ApiResponse(StatusCodes.Status200OK, "Get Region by Id successfully!!!", mapper.Map<RegionResponse>(region));
    }
}