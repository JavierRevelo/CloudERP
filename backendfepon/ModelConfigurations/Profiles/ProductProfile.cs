using AutoMapper;
using backendfepon.DTOs.ProductDTOs;
using backendfepon.Models;

namespace backendfepon.ModelConfigurations.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // Mapping from Product to ProductDTO
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Product_Id))
                .ForMember(dest => dest.stateid, opt => opt.MapFrom(src => src.State_Id))
                .ForMember(dest => dest.category, opt => opt.MapFrom(src => src.Category.Description))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.Economic_Value))
                .ForMember(dest => dest.quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.label, opt => opt.MapFrom(src => src.Label))
                .ForMember(dest => dest.provider, opt => opt.MapFrom(src => src.Provider.Name));

            // Mapping from CreateUpdateProductDTO to Product
            CreateMap<CreateUpdateProductDTO, Product>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.description))
                .ForMember(dest => dest.Economic_Value, opt => opt.MapFrom(src => src.price))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.quantity))
                .ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.label))
                .ForMember(dest => dest.State, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Provider, opt => opt.Ignore());
        }
    }
}
