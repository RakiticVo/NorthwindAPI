using AutoMapper;
using NorthwindApi.Application.DTOs.Auth;
using NorthwindApi.Application.DTOs.Category;
using NorthwindApi.Application.DTOs.Product;
using NorthwindApi.Application.DTOs.Supplier;
using NorthwindApi.Domain.Entities;

namespace NorthwindApi.Application.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Mapping cho Auth
        CreateMap<RegisterUserRequest, User>()
            .ForMember(dest => dest.HashedPassword, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "admin"))
            .ForMember(dest => dest.RowVersion, opt => opt.Ignore());
        CreateMap<RegisterResponse, User>().ReverseMap();
        CreateMap<LoginRequest, User>();
        CreateMap<UserTokenRequest, UserToken>()
            .ForMember(dest => dest.RowVersion, opt => opt.Ignore());
            
        // Mapping cho Product
        CreateMap<Product, ProductResponse>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : null))
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.CompanyName : null))
            .ReverseMap();
        CreateMap<CreateProductRequest, Product>()
            .ForMember(dest => dest.RowVersion, opt => opt.Ignore()); // bỏ qua khi create
        CreateMap<UpdateProductRequest, Product>()
            .ForMember(dest => dest.RowVersion, opt => opt.Ignore()); // bỏ qua khi update

        // Mapping cho Category
        CreateMap<Category, CategoryResponse>().ReverseMap();
        CreateMap<CreateCategoryRequest, Category>()
            .ForMember(dest => dest.Picture, opt => opt.Ignore())
            .ForMember(dest => dest.RowVersion, opt => opt.Ignore());
        CreateMap<UpdateCategoryRequest, Category>()
            .ForMember(dest => dest.Picture, opt => opt.Ignore())
            .ForMember(dest => dest.RowVersion, opt => opt.Ignore());
       
        // Mapping cho Supplier
        CreateMap<Supplier, SupplierResponse>().ReverseMap();
        CreateMap<CreateSupplierRequest, Supplier>()
            .ForMember(dest => dest.RowVersion, opt => opt.Ignore());
        CreateMap<UpdateSupplierRequest, Supplier>()
            .ForMember(dest => dest.RowVersion, opt => opt.Ignore());
    }
}