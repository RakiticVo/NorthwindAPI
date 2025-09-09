using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Shipper;

namespace NorthwindApi.Application.Features.Shipper.Commands;

public record UpdateShipperCommand(UpdateShipperRequest UpdateShipperRequest) : ICommand<ApiResponse>;

internal class UpdateShipperCommandHandler(
    ICrudService<Domain.Entities.Shipper, int> crudService,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<UpdateShipperCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(UpdateShipperCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var existingShipper = await crudService.GetByIdAsync(command.UpdateShipperRequest.Id);
            if (existingShipper is null) return new ApiResponse(StatusCodes.Status404NotFound, "Shipper not found!!!");
            
            mapper.Map(command.UpdateShipperRequest, existingShipper);
            await crudService.UpdateAsync(existingShipper, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            var shipperDto = mapper.Map<ShipperResponse>(existingShipper);
            return new ApiResponse(StatusCodes.Status200OK, "Shipper updated successfully!!!", shipperDto);
        }
    }
}