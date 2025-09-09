using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Category;

namespace NorthwindApi.Application.Features.Category.Commands;

public record UpdateCategoryCommand(UpdateCategoryRequest UpdateCategoryRequest) : ICommand<ApiResponse>;

internal class UpdateCategoryCommandHandler(
    ICrudService<Domain.Entities.Category, int> crudService,
    IMapper mapper,
    IUnitOfWork unitOfWork
) : ICommandHandler<UpdateCategoryCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(UpdateCategoryCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var existingCategory = await crudService.GetByIdAsync(command.UpdateCategoryRequest.Id);
            if (existingCategory == null) return new ApiResponse(StatusCodes.Status404NotFound, "Category not found!!!");
            
            mapper.Map(command.UpdateCategoryRequest, existingCategory);
            await crudService.UpdateAsync(existingCategory, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            var categoryDto = mapper.Map<CategoryResponse>(existingCategory);
            return new ApiResponse(StatusCodes.Status200OK, "Category updated successfully!!!", categoryDto);
        }
    }
}