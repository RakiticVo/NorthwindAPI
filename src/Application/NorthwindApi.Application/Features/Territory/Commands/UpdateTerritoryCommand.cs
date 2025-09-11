using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Territory;

namespace NorthwindApi.Application.Features.Territory.Commands;

public record UpdateTerritoryCommand(UpdateTerritoryRequest UpdateTerritoryRequest) : ICommand<ApiResponse>;

internal class UpdateTerritoryCommandHandler(
    ICrudService<Domain.Entities.Territory, string> crudService,
    IUnitOfWork unitOfWork,
    IMapper mapper    
) : ICommandHandler<UpdateTerritoryCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(UpdateTerritoryCommand command, CancellationToken cancellationToken = default)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async token =>
        {
            var existingTerritory = await crudService.GetByIdAsync(command.UpdateTerritoryRequest.Id);
            if (existingTerritory is null) return new ApiResponse(StatusCodes.Status404NotFound, "Territory not found!!!");
        
            mapper.Map(command.UpdateTerritoryRequest, existingTerritory);
            await crudService.UpdateAsync(existingTerritory, token);
            var territoryDto = mapper.Map<TerritoryResponse>(existingTerritory);
            return new ApiResponse(StatusCodes.Status200OK, "Territory updated successfully!!!", territoryDto);
        }, cancellationToken: cancellationToken);
    }
}