using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Queries;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Territory;

namespace NorthwindApi.Application.Features.Territory.Queries;

public record GetTerritories : IQuery<ApiResponse>;

internal class GetTerritoriesHandler(
    ICrudService<Domain.Entities.Territory, string> crudService,
    IMapper mapper
) : IQueryHandler<GetTerritories, ApiResponse>
{
    public async Task<ApiResponse?> HandleAsync(GetTerritories query, CancellationToken cancellationToken = default)
    {
        var territories = await crudService.GetAsync();
        return territories.Count == 0
            ? new ApiResponse(StatusCodes.Status404NotFound, "No Territories found!!!")
            : new ApiResponse(StatusCodes.Status200OK, "Get Territories successfully!!!", mapper.Map<List<TerritoryResponse>>(territories));
    }
}