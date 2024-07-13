namespace backendfepon.DTOs.AccountingAccountDTOs
{
    public class CreateUpdateAccountingAccountDTO
    {
        public string accountType { get; set; }
        public string accountName { get; set; }
        public decimal currentValue { get; set; }
        public DateTime date { get; set; }
        public decimal initialBalance { get; set; } 
        //public string accountingAccountStatus { get; set; }
    }
}
