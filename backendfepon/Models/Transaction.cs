namespace backendfepon.Models
{
    public class Transaction
    {
      
            public int Transaction_Id { get; set; }
            public int Transaction_Type_Id{ get; set; }
            public DateTime Date { get; set; }
            public int Origin_Account { get; set; }
            public int Destination_Account { get; set; }
            public decimal Value { get; set; }
            public string Reason { get; set; }

            
            public EventIncome EventIncome { get; set; }

            public EventExpense EventExpense { get; set; }
           // public AccountingAccount OriginAccount { get; set; }
        
        //public AccountingAccount DestinationAccount { get; set; }
        public TransactionType TransactionType { get; set; }


        //cypher
        public CAccountingAccount DestinationAccount { get; set; }
        public CAccountingAccount OriginAccount { get; set; }

    }
}
