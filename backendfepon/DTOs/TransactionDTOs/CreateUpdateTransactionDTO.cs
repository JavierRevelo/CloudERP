namespace backendfepon.DTOs.TransactionDTOs
{
    public class CreateUpdateTransactionDTO
    {
        public DateTime Date { get; set; }

        public string transactionType { get; set; }
        public string Origin_Account { get; set; }
        public string Destination_Account { get; set; }
        public decimal Value { get; set; }
        public string Reason { get; set; }
    }
}
