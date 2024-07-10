namespace backendfepon.DTOs.TransactionDTOs
{
    public class TransactionDTO
    {
        public int id { get; set; }
        public string transactionType { get; set; }
        public string date { get; set; }
        public string originAccount { get; set; }
        public string destinationAccount { get; set; }
        public decimal value { get; set; }
        public string description { get; set; }
    }
}
