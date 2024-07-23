namespace backendfepon.Models
{
    public class Permission
    {
        public int Permission_Id { get; set; }
        public int Status_Id { get; set; }
        public string Request { get; set; }
        // public string Request_Status { get; set; }

        // public Event Event { get; set; }

        public FinancialRequestState FinancialRequestState { get; set; }
        public ICollection<Event> Events { get; set; }

    }
}
