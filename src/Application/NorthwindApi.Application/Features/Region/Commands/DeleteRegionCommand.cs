using System.Data;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.Common.Response;

namespace NorthwindApi.Application.Features.Region.Commands;

public record DeleteRegionCommand(int RegionId) : ICommand<ApiResponse>;

internal class DeleteRegionCommandHandler(
    ICrudService<Domain.Entities.Region, int> crudService,
    IUnitOfWork unitOfWork
) : ICommandHandler<DeleteRegionCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(DeleteRegionCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var existingRegion = await crudService.GetByIdAsync(command.RegionId);
            if (existingRegion == null) return new ApiResponse(StatusCodes.Status404NotFound, "Region not found!!!");
            
            await crudService.DeleteAsync(existingRegion, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return new ApiResponse(StatusCodes.Status200OK, "Region deleted successfully!!!");
        }
    }
}