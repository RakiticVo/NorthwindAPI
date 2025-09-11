using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.Common.Response;
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
        return await unitOfWork.ExecuteInTransactionAsync(async token =>
        {
            var supplier = mapper.Map<Domain.Entities.Supplier>(command.CreateSupplierRequest);
            var suppliers = await crudService.GetAsync();
            if (suppliers.Any(supplierItem => supplierItem.CompanyName == supplier.CompanyName)) 
                return new ApiResponse(StatusCodes.Status409Conflict, "Supplier already exists!!!");
            
            await crudService.AddAsync(supplier, token);
            var newSupplier = await repository
                .FirstOrDefaultAsync(repository.GetQueryableSet()
                    .Where(x => x.CompanyName == supplier.CompanyName));
            var supplierDto = mapper.Map<SupplierResponse>(newSupplier);
            return new ApiResponse(StatusCodes.Status201Created, "Supplier created successfully!!!", supplierDto);
        }, cancellationToken: cancellationToken);
    }
}