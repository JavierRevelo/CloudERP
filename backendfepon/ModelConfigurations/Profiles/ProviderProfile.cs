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
                .ForMember(dest => dest.stateid, opt => opt.MapFrom(src => src.State_Id))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email));

            // Mapping from CreateUpdateProviderDTO to Provider
            CreateMap<CreateUpdateProviderDTO, Provider>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.name))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.phone))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.email))
                .ForMember(dest => dest.Products, opt => opt.Ignore())
                .ForMember(dest => dest.State, opt => opt.Ignore());
        }
    }
}
