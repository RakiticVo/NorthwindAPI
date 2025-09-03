using AutoMapper;
using NorthwindApi.Application.DTOs.Auth;
using NorthwindApi.Application.DTOs.Product;
using NorthwindApi.Domain.Entities;

namespace NorthwindApi.Application.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Mapping cho Auth
        CreateMap<RegisterRequest, User>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserRoleCode, opt => opt.MapFrom(src => src.RoleCode))
            .ForMember(dest => dest.HashedPassword, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "admin"))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.RowVersion, opt => opt.Ignore())
            .ForMember(dest => dest.UserTokens, opt => opt.Ignore());
        CreateMap<RegisterResponse, User>().ReverseMap();
        CreateMap<LoginRequest, User>();
        CreateMap<UserTokenRequest, UserToken>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.RowVersion, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());
        CreateMap<UpdateUserRequest, User>();
            
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