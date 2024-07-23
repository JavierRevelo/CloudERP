using AutoMapper;
using backendfepon.DTOs.PermissionDTOs;
using backendfepon.Models;

namespace backendfepon.ModelConfigurations.Profiles
{

    public class PermissionProfile : Profile
    {
        public PermissionProfile()
        {
            // Mapeo de Permission a PermissionDTO
            CreateMap<Permission, PermissionDTO>()
                .ForMember(dest => dest.Permission_Id, opt => opt.MapFrom(src => src.Permission_Id))
                .ForMember(dest => dest.Request, opt => opt.MapFrom(src => src.Request))
                .ForMember(dest => dest.Request_Status, opt => opt.MapFrom(src => src.FinancialRequestState.State_Description));

            // Mapeo de CreateUpdatePermissionDTO a Permission
            CreateMap<CreateUpdatePermissionDTO, Permission>()
                .ForMember(dest => dest.Request, opt => opt.MapFrom(src => src.Request))
                .ForMember(dest => dest.Status_Id, opt => opt.Ignore());
        }
    }

}
