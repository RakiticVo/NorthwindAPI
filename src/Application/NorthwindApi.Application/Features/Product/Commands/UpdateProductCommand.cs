using System.Data;
using AutoMapper;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.DTOs.Product;

namespace NorthwindApi.Application.Features.Product.Commands;

public record UpdateProductCommand(int Id, UpdateProductRequest UpdateProductRequest) : ICommand<ApiResponse>
{
    public UpdateProductRequest UpdateProductRequest { get; set; } = UpdateProductRequest;
}

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
            var existingProduct = await crudService.GetByIdAsync(command.Id);
            if (existingProduct == null)
                throw new Exception("Product not found");
            mapper.Map(command.UpdateProductRequest, existingProduct);
            await crudService.UpdateAsync(existingProduct, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken); 
            var productDto = mapper.Map<ProductDto>(existingProduct);
            return new ApiResponse(200, "Product updated successfully", productDto);
        }
    }
}