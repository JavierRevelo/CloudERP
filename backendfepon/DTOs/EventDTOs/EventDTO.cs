namespace backendfepon.DTOs.EventDTOs
{
    public class EventDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public string status { get; set; }
        public string description { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public decimal budget { get; set; }
        public string budgetStatus { get; set; }
        public string location { get; set; }
        public string income { get; set; }
        //public string Hiring { get; set; }
    }
}
