using AutoMapper;
using backendfepon.DTOs.TransactionDTOs;
using backendfepon.Models;
using System.Globalization;

namespace backendfepon.ModelConfigurations.Profiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<CreateUpdateTransactionDTO, Transaction>()
                .ForMember(dest => dest.Origin_Account, opt => opt.Ignore())
                .ForMember(dest => dest.Destination_Account, opt => opt.Ignore())
                .ForMember(dest => dest.TransactionType, opt => opt.Ignore());

            CreateMap<Transaction, TransactionDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Transaction_Id))
                .ForMember(dest => dest.originAccount, opt => opt.MapFrom(src => src.OriginAccount.Account_Name))
                .ForMember(dest => dest.destinationAccount, opt => opt.MapFrom(src => src.DestinationAccount.Account_Name))
                .ForMember(dest => dest.transactionType, opt => opt.MapFrom(src => src.TransactionType.Transaction_Type_Name)) // Assuming TransactionType has a name property
                .ForMember(dest => dest.date, opt => opt.MapFrom(src => src.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture))); // Adjust date format as needed
        }
    }
}
