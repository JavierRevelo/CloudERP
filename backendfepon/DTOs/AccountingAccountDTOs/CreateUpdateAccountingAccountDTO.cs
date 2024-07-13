namespace backendfepon.DTOs.AccountingAccountDTOs
{
    public class CreateUpdateAccountingAccountDTO
    {
        public string accountType { get; set; }
        public string accountName { get; set; }
        public decimal currentValue { get; set; }
        public string date { get; set; }
        public decimal initialBalance { get; set; } 
        //public string accountingAccountStatus { get; set; }
    }
}
