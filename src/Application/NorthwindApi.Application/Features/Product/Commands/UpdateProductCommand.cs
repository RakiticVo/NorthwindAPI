using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.DTOs.Product;

namespace NorthwindApi.Application.Features.Product.Commands;

public record UpdateProductCommand(UpdateProductRequest UpdateProductRequest) : ICommand<ApiResponse>;

internal class UpdateProductCommandHandler(
    ICrudService<Domain.Entities.Product, int> crudService,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<UpdateProductCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(UpdateProductCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var existingProduct = await crudService.GetByIdAsync(command.UpdateProductRequest.Id);
            if (existingProduct == null) return new ApiResponse(StatusCodes.Status404NotFound, "Product not found");
            
            mapper.Map(command.UpdateProductRequest, existingProduct);
            await crudService.UpdateAsync(existingProduct, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken); 
            var productDto = mapper.Map<ProductResponse>(existingProduct);
            return new ApiResponse(StatusCodes.Status200OK, "Product updated successfully", productDto);
        }
    }
}