using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Region;

namespace NorthwindApi.Application.Features.Region.Commands;

public record UpdateRegionCommand(UpdateRegionRequest UpdateRegionRequest) : ICommand<ApiResponse>;

internal class UpdateRegionCommandHandler(
    ICrudService<Domain.Entities.Region, int> crudService,
    IUnitOfWork unitOfWork,
    IMapper mapper    
) : ICommandHandler<UpdateRegionCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(UpdateRegionCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var existingRegion = await crudService.GetByIdAsync(command.UpdateRegionRequest.Id);
            if (existingRegion is null) return new ApiResponse(StatusCodes.Status404NotFound, "Region not found!!!");
        
            mapper.Map(command.UpdateRegionRequest, existingRegion);
            await crudService.UpdateAsync(existingRegion, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            var regionDto = mapper.Map<RegionResponse>(existingRegion);
            return new ApiResponse(StatusCodes.Status200OK, "Region updated successfully!!!", regionDto);
        }
    }
}