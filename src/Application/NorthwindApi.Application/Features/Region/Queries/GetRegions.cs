using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Queries;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Region;

namespace NorthwindApi.Application.Features.Region.Queries;

public record GetRegions : IQuery<ApiResponse>;

internal class GetRegionsHandler(
    ICrudService<Domain.Entities.Region, int> crudService,
    IMapper mapper
) : IQueryHandler<GetRegions, ApiResponse>
{
    public async Task<ApiResponse?> HandleAsync(GetRegions query, CancellationToken cancellationToken = default)
    {
        var regions = await crudService.GetAsync();
        return regions.Count == 0
            ? new ApiResponse(StatusCodes.Status404NotFound, "No Regions found!!!")
            : new ApiResponse(StatusCodes.Status200OK, "Get Regions successfully!!!", mapper.Map<List<RegionResponse>>(regions));
    }
}