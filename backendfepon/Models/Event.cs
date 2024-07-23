namespace backendfepon.Models
{
    public class Event
    {
        public int Event_Id { get; set; }
        public int? State_Id { get; set; }
        public int? Financial_Request_Id { get; set; }
        public int? Permission_Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        public string Budget_Status { get; set; }
        public string Event_Location { get; set; }
        public decimal Income { get; set; }
        public int Event_Status_Id { get; set; }

        public EventState State { get; set; }

        public Permission? Permission { get; set; }
        public FinancialRequest? Financial_Request { get; set; }

        public State State_State { get; set; }

        //public EventIncome EventIncome { get; set; }
        //  public ICollection<Responsible> Responsibles { get; set; }

        // public ICollection<EventExpense> EventExpenses { get; set; }
    }
}
