namespace backendfepon.Models
{
    public class Provider
    {
        public int Provider_Id { get; set; }
        public int State_Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public ICollection<EventExpense> EventExpenses { get; set; }

        public ICollection<Product> Products { get; set; }

        public State State { get; set; }

    }
}
