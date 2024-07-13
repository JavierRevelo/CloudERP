namespace backendfepon.DTOs.FinantialRequestDTOs
{
    public class CreateUpdateFinantialRequestDTO
    {
        //public string AdministrativeMember_Name { get; set; }
        public string requestStatusName { get; set; }
        public string eventName { get; set; }
        public decimal value { get; set; }
        public string reason { get; set; }

    }
}
