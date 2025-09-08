using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.DTOs.Supplier;

namespace NorthwindApi.Application.Features.Supplier.Commands;

public record CreateSupplierCommand(CreateSupplierRequest CreateSupplierRequest) : ICommand<ApiResponse>;

internal class CreateSupplierCommandHandler(
    ICrudService<Domain.Entities.Supplier, int> crudService,
    IRepository<Domain.Entities.Supplier, int> repository,
    IMapper mapper,
    IUnitOfWork unitOfWork
) : ICommandHandler<CreateSupplierCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(CreateSupplierCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var existingSupplier = mapper.Map<Domain.Entities.Supplier>(command.CreateSupplierRequest);
            var suppliers = await crudService.GetAsync();
            if (suppliers.Any(supplierItem => supplierItem.CompanyName == existingSupplier.CompanyName)) 
                return new ApiResponse(StatusCodes.Status409Conflict, "Supplier already exists");
            await crudService.AddAsync(existingSupplier, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            var newSupplier = await repository
                .FirstOrDefaultAsync(repository.GetQueryableSet()
                    .Where(x => x.CompanyName == existingSupplier.CompanyName));
            var supplierDto = mapper.Map<SupplierResponse>(newSupplier);
            return new ApiResponse(StatusCodes.Status201Created, "Create Supplier successfully!!!", supplierDto);
        }
    }
}