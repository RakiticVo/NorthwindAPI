using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.DTOs.Category;

namespace NorthwindApi.Application.Features.Category.Commands;

public record CreateCategoryCommand(CreateCategoryRequest CreateCategoryRequest) : ICommand<ApiResponse>;

internal class CreateCategoryCommandHandler(
    ICrudService<Domain.Entities.Category, int> crudService,
    IRepository<Domain.Entities.Category, int> repository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<CreateCategoryCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(CreateCategoryCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var category = mapper.Map<Domain.Entities.Category>(command.CreateCategoryRequest);
            await crudService.AddAsync(category, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            var newCategory = await repository
                .FirstOrDefaultAsync(repository.GetQueryableSet()
                    .Where(x => x.CategoryName == category.CategoryName));
            var categoryDto = mapper.Map<CategoryDto>(newCategory);
            return new ApiResponse(StatusCodes.Status201Created, "Create Category successfully!!!", categoryDto);
        }
    }
}