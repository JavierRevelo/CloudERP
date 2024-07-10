using AutoMapper;
using backendfepon.DTOs.AssociationDTOs;
using backendfepon.Models;

namespace backendfepon.ModelConfigurations.Profiles
{
    public class AssociationProfile : Profile
    {
        public AssociationProfile()
        {

            CreateMap<Association, AssociationDTO>()
           .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Association_Id))
           .ForMember(dest => dest.state_id, opt => opt.MapFrom(src => src.State_Id))
           .ForMember(dest => dest.mission, opt => opt.MapFrom(src => src.Mission))
           .ForMember(dest => dest.vision, opt => opt.MapFrom(src => src.Vision));

            CreateMap<CreateUpdateAssociationDTO, Association>()
                .ForMember(dest => dest.Mission, opt => opt.MapFrom(src => src.Mission))
                .ForMember(dest => dest.Vision, opt => opt.MapFrom(src => src.Vision));
        }
    }
}
