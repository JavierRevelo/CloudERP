namespace backendfepon.Models
{
    public class FinancialRequestState
    {
        public int Request_State_Id { get; set; }
        public string State_Description { get; set; }

        public ICollection<FinancialRequest> FinancialRequests { get; set; }
        public ICollection<Permission> Permissions { get; set; }
    }
}
