using AutoMapper;
using backendfepon.DTOs.ContributorDTO;
using backendfepon.Models;
using System.Globalization;

namespace backendfepon.ModelConfigurations.Profiles
{
    public class ContributorProfile : Profile
    {
        public ContributorProfile()
        {
            // Mapping from Contributor to ContributorDTO
            CreateMap<Contributor, ContributorDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Contributor_Id))
                .ForMember(dest => dest.State_id, opt => opt.MapFrom(src => src.State_Id))
                .ForMember(dest => dest.Plan, opt => opt.MapFrom(src => src.ContributionPlan.Name))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Contributor_Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.ContributionPlan.Economic_Value))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Faculty, opt => opt.MapFrom(src => src.Faculty.Faculty_Name))
                .ForMember(dest => dest.Career, opt => opt.MapFrom(src => src.Career.Career_Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            /* CreateMap<CreateUpdateContributorDTO, Contributor>()
          .ForMember(dest => dest.Contributor_Date, opt => opt.MapFrom(src => src.Date))
          .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
          .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
          .ForMember(dest => dest.Faculty_Id, opt => opt.Ignore())
          .ForMember(dest => dest.Career_Id, opt => opt.Ignore())
          .ForMember(dest => dest.Plan_Id, opt => opt.Ignore())
          .ForMember(dest => dest.Transaction_Id, opt => opt.Ignore());


             // Mapping from CreateUpdateContributorDTO to Contributor
             CreateMap<CreateUpdateContributorDTO, Contributor>()
                 .ForMember(dest => dest.Plan_Id, opt => opt.Ignore());
                // .ForMember(dest => dest.Student_Id, opt => opt.Ignore());

            CreateMap<CreateUpdateContributorDTO, Contributor>()
           .ForMember(dest => dest.Contributor_Date, opt => opt.MapFrom(src => src.Date))
           .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
           .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
           .ForMember(dest => dest.Faculty_Id, opt => opt.Ignore())
           .ForMember(dest => dest.Career_Id, opt => opt.Ignore())
           .ForMember(dest => dest.Plan_Id, opt => opt.Ignore())
           .ForMember(dest => dest.State_Id, opt => opt.Ignore());*/


            CreateMap<CreateUpdateContributorDTO, Contributor>()
          .ForMember(dest => dest.Contributor_Date, opt => opt.MapFrom(src => src.Date))
          .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
          .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
          .ForMember(dest => dest.Faculty, opt => opt.Ignore())
          .ForMember(dest => dest.Career, opt => opt.Ignore())
          .ForMember(dest => dest.ContributionPlan, opt => opt.Ignore())
          .ForMember(dest => dest.Faculty_Id, opt => opt.Ignore())
          .ForMember(dest => dest.Career_Id, opt => opt.Ignore())
          .ForMember(dest => dest.Plan_Id, opt => opt.Ignore())
          .ForMember(dest => dest.State_Id, opt => opt.Ignore());
        }
    }
}
