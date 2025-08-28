using AutoMapper;
using NorthwindApi.Application.DTOs.Product;
using NorthwindApi.Domain.Entities;

namespace NorthwindApi.Application.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Mapping cho Product
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<CreateProductRequest, Product>()
            .ForMember(dest => dest.RowVersion, opt => opt.Ignore()); // bỏ qua khi create
        CreateMap<UpdateProductRequest, Product>()
            .ForMember(dest => dest.RowVersion, opt => opt.Ignore()); // bỏ qua khi update

        // // Mapping cho các Entity và DTO khác
        // CreateMap<Category, CategoryDto>().ReverseMap();
        // CreateMap<CreateCategoryRequest, Category>();
        // CreateMap<UpdateCategoryRequest, Category>();
        //
        // CreateMap<Supplier, SupplierDto>().ReverseMap();
        // CreateMap<CreateSupplierRequest, Supplier>();
        // CreateMap<UpdateSupplierRequest, Supplier>();
    }
}