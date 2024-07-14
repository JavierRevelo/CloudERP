using AutoMapper;
using backendfepon.Cypher;
using backendfepon.DTOs.TransactionDTOs;
using backendfepon.Models;
using System.Globalization;

namespace backendfepon.ModelConfigurations.Profiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            /*
            CreateMap<CreateUpdateTransactionDTO, Transaction>()
                .ForMember(dest => dest.Origin_Account, opt => opt.Ignore())
                .ForMember(dest => dest.Destination_Account, opt => opt.Ignore())
                .ForMember(dest => dest.TransactionType, opt => opt.Ignore());

            CreateMap<Transaction, TransactionDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Transaction_Id))
                .ForMember(dest => dest.originAccount, opt => opt.MapFrom(src => src.OriginAccount.Account_Name))
                .ForMember(dest => dest.destinationAccount, opt => opt.MapFrom(src => src.DestinationAccount.Account_Name))
                .ForMember(dest => dest.transactionType, opt => opt.MapFrom(src => src.TransactionType.Transaction_Type_Name)) // Assuming TransactionType has a name property
                .ForMember(dest => dest.date, opt => opt.MapFrom(src => src.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture))); // Adjust date format as needed*/

            CypherAES cy = new CypherAES();
            CreateMap<CreateUpdateTransactionDTO, Transaction>()
            .ForMember(dest => dest.Origin_Account, opt => opt.Ignore())
            .ForMember(dest => dest.Destination_Account, opt => opt.Ignore())
            .ForMember(dest => dest.Transaction_Type_Id, opt => opt.Ignore())
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.date))
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.value))
            .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.description))
            .ForMember(dest => dest.TransactionType, opt => opt.Ignore())
            .ForMember(dest => dest.OriginAccount, opt => opt.Ignore())
            .ForMember(dest => dest.DestinationAccount, opt => opt.Ignore()); // Ignorar la propiedad de navegación

            CreateMap<Transaction, TransactionDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Transaction_Id))
                .ForMember(dest => dest.originAccount, opt => opt.MapFrom(src => src.OriginAccount.Account_Name))
                .ForMember(dest => dest.destinationAccount, opt => opt.MapFrom(src => src.DestinationAccount.Account_Name))
                .ForMember(dest => dest.transactionType, opt => opt.MapFrom(src => src.TransactionType.Transaction_Type_Name)) // Assuming TransactionType has a name property
                .ForMember(dest => dest.date, opt => opt.MapFrom(src => src.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture))) // Adjust date format as needed
                .ForMember(dest => dest.value, opt => opt.MapFrom(src => src.Value))
                .ForMember(dest => dest.description, opt => opt.MapFrom(src => src.Reason));

        }
}
}
