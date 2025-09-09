using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Territory;

namespace NorthwindApi.Application.Features.Territory.Commands;

public record CreateTerritoryCommand(CreateTerritoryRequest CreateTerritoryRequest) : ICommand<ApiResponse>;

internal class CreateTerritoryCommandHandler(
    ICrudService<Domain.Entities.Territory, string> crudService,
    IRepository<Domain.Entities.Territory, string> repository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<CreateTerritoryCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(CreateTerritoryCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var territory = mapper.Map<Domain.Entities.Territory>(command.CreateTerritoryRequest);
            var territories = await crudService.GetAsync();
            if (territories.Any(territoryItem => territoryItem.TerritoryDescription == territory.TerritoryDescription)) 
                return new ApiResponse(StatusCodes.Status409Conflict, "Territory already exists!!!");

            await crudService.AddAsync(territory, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            var newTerritory = await repository
                .FirstOrDefaultAsync(repository.GetQueryableSet()
                    .Where(x => x.TerritoryDescription == territory.TerritoryDescription));
            var territoryDto = mapper.Map<TerritoryResponse>(newTerritory);
            return new ApiResponse(StatusCodes.Status201Created, "Territory created successfully!!!", territoryDto);
        }
    }
}