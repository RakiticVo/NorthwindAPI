using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Shipper;

namespace NorthwindApi.Application.Features.Shipper.Commands;

public record CreateShipperCommand(CreateShipperRequest CreateShipperRequest) : ICommand<ApiResponse>;

internal class CreateShipperCommandHandler(
    ICrudService<Domain.Entities.Shipper, int> crudService,
    IRepository<Domain.Entities.Shipper, int> repository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<CreateShipperCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(CreateShipperCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var shipper = mapper.Map<Domain.Entities.Shipper>(command.CreateShipperRequest);
            var shippers = await crudService.GetAsync();
            if (shippers.Any(shipperItem => shipperItem.CompanyName == shipper.CompanyName)) 
                return new ApiResponse(StatusCodes.Status409Conflict, "Shipper already exists!!!");
            
            await crudService.AddAsync(shipper, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            var newShipper = await repository
                .FirstOrDefaultAsync(repository.GetQueryableSet()
                    .Where(x => x.CompanyName == shipper.CompanyName));
            var shipperDto = mapper.Map<ShipperResponse>(newShipper);
            return new ApiResponse(StatusCodes.Status201Created, "Shipper created successfully!!!", shipperDto);
        }
    }
}