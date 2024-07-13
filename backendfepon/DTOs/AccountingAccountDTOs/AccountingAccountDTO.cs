namespace backendfepon.DTOs.AccountingAccountDTOs
{
    public class AccountingAccountDTO
    {
        public int id { get; set; }
        public string accountType { get; set; }
        public string accountName { get; set; }
        public decimal currentValue { get; set; }
        public DateTime date { get; set; }
        public decimal initialBalance { get; set; }
        //apublic string accountingAccountStatus { get; set; }

    }
}
