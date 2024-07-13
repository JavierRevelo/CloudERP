using AutoMapper;
using backendfepon.DTOs.ProviderDTOs;
using backendfepon.Models;

namespace backendfepon.ModelConfigurations.Profiles
{
    public class ProviderProfile : Profile
    {
        public ProviderProfile()
        {
            // Mapping from Provider to ProviderDTO
            CreateMap<Provider, ProviderDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Provider_Id))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email));

            // Mapping from ProviderDTO to Provider
            CreateMap<ProviderDTO, Provider>()
                .ForMember(dest => dest.Provider_Id, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.name))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.phone))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.email))
                .ForMember(dest => dest.EventExpenses, opt => opt.Ignore())
                .ForMember(dest => dest.State, opt => opt.Ignore())
                .ForMember(dest => dest.Products, opt => opt.Ignore());
        }
    }
}
