using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Region;

namespace NorthwindApi.Application.Features.Region.Commands;

public record CreateRegionCommand(CreateRegionRequest CreateRegionRequest) : ICommand<ApiResponse>;

internal class CreateRegionCommandHandler(
    ICrudService<Domain.Entities.Region, int> crudService,
    IRepository<Domain.Entities.Region, int> repository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<CreateRegionCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(CreateRegionCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var region = mapper.Map<Domain.Entities.Region>(command.CreateRegionRequest);
            var regions = await crudService.GetAsync();
            if (regions.Any(regionItem => regionItem.RegionDescription == region.RegionDescription)) 
                return new ApiResponse(StatusCodes.Status409Conflict, "Region already exists!!!");

            await crudService.AddAsync(region, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            var newRegion = await repository
                .FirstOrDefaultAsync(repository.GetQueryableSet()
                    .Where(x => x.RegionDescription == region.RegionDescription));
            var regionDto = mapper.Map<RegionResponse>(newRegion);
            return new ApiResponse(StatusCodes.Status201Created, "Region created successfully!!!", regionDto);
        }
    }
}