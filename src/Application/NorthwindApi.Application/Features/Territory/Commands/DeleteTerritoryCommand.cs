using System.Data;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.Common.Response;

namespace NorthwindApi.Application.Features.Territory.Commands;

public record DeleteTerritoryCommand(string TerritoryId) : ICommand<ApiResponse>;

internal class DeleteTerritoryCommandHandler(
    ICrudService<Domain.Entities.Territory, string> crudService,
    IUnitOfWork unitOfWork
) : ICommandHandler<DeleteTerritoryCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(DeleteTerritoryCommand command, CancellationToken cancellationToken = default)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async token =>
        {
            var existingTerritory = await crudService.GetByIdAsync(command.TerritoryId);
            if (existingTerritory == null) return new ApiResponse(StatusCodes.Status404NotFound, "Territory not found!!!");
            
            await crudService.DeleteAsync(existingTerritory, token);
            return new ApiResponse(StatusCodes.Status200OK, "Territory deleted successfully!!!");
        }, cancellationToken: cancellationToken);
    }
}