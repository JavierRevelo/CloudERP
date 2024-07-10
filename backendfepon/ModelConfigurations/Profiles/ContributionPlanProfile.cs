using AutoMapper;
using backendfepon.DTOs.ContributionPlanDTOs;
using backendfepon.Models;

namespace backendfepon.ModelConfigurations.Profiles
{
    public class ContributionPlanProfile : Profile
    {
        public ContributionPlanProfile()
        {
            // Mapping from ContributionPlan to ContributionPlanDTO
            CreateMap<ContributionPlan, ContributionPlanDTO>()
                .ForMember(dest => dest.academicPeriod, opt => opt.MapFrom(src => src.AcademicPeriod.Academic_Period_Name));

            // Mapping from CreateUpdateContributionPlanDTO to ContributionPlan
            CreateMap<CreateUpdateContributionPlanDTO, ContributionPlan>()
                .ForMember(dest => dest.State_Id, opt => opt.Ignore())
                .ForMember(dest => dest.Academic_Period_Id, opt => opt.Ignore());
        }
    }
}
