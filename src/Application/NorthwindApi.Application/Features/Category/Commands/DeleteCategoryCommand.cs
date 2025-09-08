using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;

namespace NorthwindApi.Application.Features.Category.Commands;

public record DeleteCategoryCommand(int CategoryId) : ICommand<ApiResponse>;

internal class DeleteCategoryCommandHandler(
    ICrudService<Domain.Entities.Category, int> crudService,
    IUnitOfWork unitOfWork
) : ICommandHandler<DeleteCategoryCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(DeleteCategoryCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var existingCategory = await crudService.GetByIdAsync(command.CategoryId);
            if (existingCategory == null)
                return new ApiResponse(StatusCodes.Status404NotFound, "Category not found!!!");
            await crudService.DeleteAsync(existingCategory, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return new ApiResponse(StatusCodes.Status200OK, "Delete Category successfully!!!");
        }
    }
}