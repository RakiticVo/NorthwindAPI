using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Queries;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Territory;

namespace NorthwindApi.Application.Features.Territory.Queries;

public record GetTerritoryById(string TerritoryId) : IQuery<ApiResponse>;

internal class GetTerritoryByIdHandler(
    ICrudService<Domain.Entities.Territory, string> crudService,
    IMapper mapper
) : IQueryHandler<GetTerritoryById, ApiResponse>
{
    public async Task<ApiResponse?> HandleAsync(GetTerritoryById query, CancellationToken cancellationToken = default)
    {
        var territory = await crudService.GetByIdAsync(query.TerritoryId);
        return territory == null
            ? new ApiResponse(StatusCodes.Status404NotFound, "Territory not found!!!")
            : new ApiResponse(StatusCodes.Status200OK, "Get Territory by Id successfully!!!", mapper.Map<TerritoryResponse>(territory));
    }
}