namespace backendfepon.Models
{
    public class CAccountingAccount
    {
        public int Account_Id { get; set; }
        public int Account_Type_Id { get; set; }
        public byte[] Account_Name { get; set; }

        public byte[] Current_Value { get; set; }

        public byte[] Initial_Balance_Date { get; set; }

        public byte[] Initial_Balance { get; set; }

        public byte[] Accounting_Account_Status { get; set; }


        // Navigation properties
        public ICollection<Transaction> OriginTransactions { get; set; }
        public ICollection<Transaction> DestinationTransactions { get; set; }
        public AccountType AccountType { get; set; }
    }
}
