namespace backendfepon.DTOs.FinantialRequestDTOs
{
    public class CreateUpdateFinantialRequestDTO
    {
        //public string AdministrativeMember_Name { get; set; }
        public string Request_Status_Name { get; set; }
        public string Event_name { get; set; }
        public decimal Value { get; set; }
        public string Reason { get; set; }
    }
}
