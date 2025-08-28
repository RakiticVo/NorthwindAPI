using System.Data;
using System.Windows.Input;
using AutoMapper;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.DTOs.Product;

namespace NorthwindApi.Application.Features.Product.Commands;

public record CreateProductCommand(CreateProductRequest CreateProductRequest) : ICommand<ApiResponse>
{
    public CreateProductRequest CreateProductRequest { get; set; } = CreateProductRequest;
}

internal class CreateProductCommandHandler(
    ICrudService<Domain.Entities.Product, int> crudService,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<CreateProductCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(CreateProductCommand command, CancellationToken cancellationToken = default)
    {
        using (await unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken))
        {
            var product = mapper.Map<Domain.Entities.Product>(command.CreateProductRequest);
            await crudService.AddAsync(product, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            var productDto = mapper.Map<ProductDto>(product);
            return new ApiResponse(201, "Product created successfully", productDto);
        }
    }
}