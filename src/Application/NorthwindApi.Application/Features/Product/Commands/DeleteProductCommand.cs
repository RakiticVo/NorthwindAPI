using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;

namespace NorthwindApi.Application.Features.Product.Commands;

public record DeleteProductCommand(int Id) : ICommand<ApiResponse>;

internal class DeleteProductCommandHandler(
    ICrudService<Domain.Entities.Product, int> crudService,
    IUnitOfWork unitOfWork
) : ICommandHandler<DeleteProductCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(DeleteProductCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(cancellationToken: cancellationToken))
        {
            var existingProduct = await crudService.GetByIdAsync(command.Id);
            if (existingProduct == null) return new ApiResponse(StatusCodes.Status404NotFound, "Product not found");

            await crudService.DeleteAsync(existingProduct, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return new ApiResponse(StatusCodes.Status200OK, "Product deleted successfully");
        }
    }
}