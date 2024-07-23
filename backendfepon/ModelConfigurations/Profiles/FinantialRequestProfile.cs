using AutoMapper;
using backendfepon.DTOs.FinantialRequestDTOs;
using backendfepon.Models;

namespace backendfepon.ModelConfigurations.Profiles
{
    public class FinancialRequestProfile : Profile
    {
        public FinancialRequestProfile()
        {
            // Mapping from FinancialRequest to FinancialRequestDTO
            CreateMap<FinancialRequest, FinantialRequestDTO>()
                //.ForMember(dest => dest.AdministrativeMember_Name, opt => opt.MapFrom(src => src.AdministrativeMember.Name + ' ' + src.AdministrativeMember.Last_Name ))
                .ForMember(dest => dest.requestStatusName, opt => opt.MapFrom(src => src.Financial_Request_State.State_Description));


            // Mapping from CreateUpdateFinancialRequestDTO to FinancialRequest
            CreateMap<CreateUpdateFinantialRequestDTO, FinancialRequest>()
                //.ForMember(dest => dest.Administrative_Member_Id, opt => opt.Ignore())
                .ForMember(dest => dest.Request_Status_Id, opt => opt.Ignore());

        }
    }
}
