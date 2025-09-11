using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Supplier;

namespace NorthwindApi.Application.Features.Supplier.Commands;

public record UpdateSupplierCommand(UpdateSupplierRequest UpdateSupplierRequest) : ICommand<ApiResponse>;

internal class UpdateSupplierCommandHandler(
    ICrudService<Domain.Entities.Supplier, int> crudService,
    IMapper mapper,
    IUnitOfWork unitOfWork
) : ICommandHandler<UpdateSupplierCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(UpdateSupplierCommand command, CancellationToken cancellationToken = default)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async token =>
        {
            var existingSupplier = await crudService.GetByIdAsync(command.UpdateSupplierRequest.Id);
            if (existingSupplier == null) 
                return new ApiResponse(StatusCodes.Status404NotFound, "Supplier not found!!!");
            
            mapper.Map(command.UpdateSupplierRequest, existingSupplier);
            await crudService.UpdateAsync(existingSupplier, token);
            var supplierDto = mapper.Map<SupplierResponse>(existingSupplier);
            return new ApiResponse(StatusCodes.Status200OK, "Supplier updated successfully!!!", supplierDto);
        }, cancellationToken: cancellationToken);
    }
}