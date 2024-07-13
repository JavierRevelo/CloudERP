using AutoMapper;
using backendfepon.DTOs.AccountingAccountDTOs;
using backendfepon.Models;

namespace backendfepon.ModelConfigurations.Profiles
{
    /*
    public class AccountingAccountProfile : Profile
    {
        public AccountingAccountProfile()
        {
            // Mapping from CAccountingAccount to CAccountingAccountDTOs
            CreateMap<CAccountingAccount, CAccountingAccountDTOs>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Account_Id))
                .ForMember(dest => dest.stateid, opt => opt.MapFrom(src => src.Accounting_Account_Status_Id))
                .ForMember(dest => dest.accountType, opt => opt.MapFrom(src => src.AccountType.Name))
                .ForMember(dest => dest.accountName, opt => opt.MapFrom(src => EncryptionHelper.DecryptStringFromBytes_Aes(src.Account_Name, EncryptionHelper._key, EncryptionHelper._iv)))
                .ForMember(dest => dest.currentValue, opt => opt.MapFrom(src => BitConverter.ToDecimal(src.Current_Value, 0)))
                .ForMember(dest => dest.date, opt => opt.MapFrom(src => EncryptionHelper.DecryptStringFromBytes_Aes(src.Initial_Balance_Date, EncryptionHelper._key, EncryptionHelper._iv)))
                .ForMember(dest => dest.initialBalance, opt => opt.MapFrom(src => BitConverter.ToDecimal(src.Initial_Balance, 0)));

            // Mapping from CreateUpdateAccountingAccountDTO to CAccountingAccount
            CreateMap<CreateUpdateAccountingAccountDTO, CAccountingAccount>()
                .ForMember(dest => dest.Account_Type_Id, opt => opt.MapFrom(src => src.Account_Type_Id))
                .ForMember(dest => dest.Account_Name, opt => opt.MapFrom(src => EncryptionHelper.EncryptStringToBytes_Aes(src.Account_Name, EncryptionHelper._key, EncryptionHelper._iv)))
                .ForMember(dest => dest.Current_Value, opt => opt.MapFrom(src => BitConverter.GetBytes(src.Current_Value)))
                .ForMember(dest => dest.Initial_Balance_Date, opt => opt.MapFrom(src => EncryptionHelper.EncryptStringToBytes_Aes(src.Initial_Balance_Date.ToString("yyyy-MM-dd"), EncryptionHelper._key, EncryptionHelper._iv)))
                .ForMember(dest => dest.Initial_Balance, opt => opt.MapFrom(src => BitConverter.GetBytes(src.Initial_Balance)))
                .ForMember(dest => dest.Accounting_Account_Status_Id, opt => opt.Ignore())
                .ForMember(dest => dest.OriginTransactions, opt => opt.Ignore())
                .ForMember(dest => dest.DestinationTransactions, opt => opt.Ignore())
                .ForMember(dest => dest.AccountType, opt => opt.Ignore())
                .ForMember(dest => dest.State, opt => opt.Ignore());
        }
    
    }
    */
}
