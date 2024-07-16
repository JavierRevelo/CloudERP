using AutoMapper;
using backendfepon.Cypher;
using backendfepon.DTOs.EventDTOs;
using backendfepon.Models;

namespace backendfepon.ModelConfigurations.Profiles
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {

           
            CreateMap<Event, EventDTO>()
           .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Event_Id))
           .ForMember(dest => dest.stateid, opt => opt.MapFrom(src => src.Event_Status_Id))
           .ForMember(dest => dest.title, opt => opt.MapFrom(src => src.Title))
           .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.State.Event_State_Name))
           .ForMember(dest => dest.description, opt => opt.MapFrom(src => src.Description))
           .ForMember(dest => dest.startDate, opt => opt.MapFrom(src => src.Start_Date.ToString("yyyy-MM-dd")))
           .ForMember(dest => dest.endDate, opt => opt.MapFrom(src => src.End_Date.ToString("yyyy-MM-dd")))
           .ForMember(dest => dest.budget, opt => opt.MapFrom(src => src.Income))
           .ForMember(dest => dest.budgetStatus, opt => opt.MapFrom(src => src.Budget_Status))
           .ForMember(dest => dest.location, opt => opt.MapFrom(src => src.Event_Location))
           .ForMember(dest => dest.income, opt => opt.MapFrom(src => src.Income.ToString("F2"))); // Format decimal as string with 2 decimal places


            CreateMap<CreateUpdateEventDTO, Event>()
           .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.title ?? string.Empty))
           .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.description ?? string.Empty))
           .ForMember(dest => dest.Start_Date, opt => opt.MapFrom(src => DateTime.Parse(src.startDate ?? DateTime.MinValue.ToString())))
           .ForMember(dest => dest.End_Date, opt => opt.MapFrom(src => DateTime.Parse(src.endDate ?? DateTime.MinValue.ToString())))
           .ForMember(dest => dest.Budget_Status, opt => opt.MapFrom(src => src.budgetStatus ?? string.Empty))
           .ForMember(dest => dest.Event_Location, opt => opt.MapFrom(src => src.location ?? string.Empty))
           .ForMember(dest => dest.Income, opt => opt.MapFrom(src => src.income))
           .ForMember(dest => dest.State, opt => opt.Ignore())
           .ForMember(dest => dest.Financial_Request, opt => opt.Ignore())
           .ForMember(dest => dest.Permission, opt => opt.Ignore())
           .ForMember(dest => dest.State_State, opt => opt.Ignore());
        }
    }
}
