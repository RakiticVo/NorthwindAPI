using System.Data;
using System.Windows.Input;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.Commands;
using NorthwindApi.Application.Common.Response;
using NorthwindApi.Application.DTOs.Product;

namespace NorthwindApi.Application.Features.Product.Commands;

public record CreateProductCommand(CreateProductRequest CreateProductRequest) : ICommand<ApiResponse>;

internal class CreateProductCommandHandler(
    ICrudService<Domain.Entities.Product, int> crudService,
    IRepository<Domain.Entities.Product, int> repository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<CreateProductCommand, ApiResponse>
{
    public async Task<ApiResponse> HandleAsync(CreateProductCommand command, CancellationToken cancellationToken = default)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async token =>
        {
            var product = mapper.Map<Domain.Entities.Product>(command.CreateProductRequest);
            var products = await crudService.GetAsync();
            if (products.Any(productItem => productItem.ProductName == product.ProductName)) 
                return new ApiResponse(StatusCodes.Status409Conflict, "Product already exists!!!");
            
            await crudService.AddAsync(product, token);
            var newProduct = await repository
                .FirstOrDefaultAsync(repository.GetQueryableSet()
                    .Where(x => x.ProductName == product.ProductName));
            var productDto = mapper.Map<ProductResponse>(newProduct);
            return new ApiResponse(StatusCodes.Status201Created, "Product created successfully!!!", productDto);
        }, cancellationToken: cancellationToken);
    }
}