namespace backendfepon.Models
{
    public class EventExpense
    {
        public int Expense_Id { get; set; }
        public int Transaction_Id { get; set; }
        public int Event_Id { get; set; }
        public int Responsible_Id { get; set; }


        public Responsible Responsible { get; set; }

        public Transaction Transaction { get; set; }

        public Event Event { get; set; }
    }
}
