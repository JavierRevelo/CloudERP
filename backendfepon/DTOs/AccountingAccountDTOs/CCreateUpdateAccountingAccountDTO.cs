namespace backendfepon.DTOs.AccountingAccountDTOs
{
    public class CCreateUpdateAccountingAccountDTO
    {
        public int Account_Type_Id { get; set; }
        public string Account_Name { get; set; }
        public byte[] Current_Value { get; set; }
        public byte[] Initial_Balance_Date { get; set; }
        public byte[] Initial_Balance { get; set; }
        public byte[] Accounting_Account_Status { get; set; }
    }
}
