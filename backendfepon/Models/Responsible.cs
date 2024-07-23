namespace backendfepon.Models
{
    public class Responsible
    {
        public int Responsible_Id { get; set; }
        public int Administrative_Member_Id { get; set; }
        public int Event_Id { get; set; }
        public string Event_Role { get; set; }
        public Event Event { get; set; }
        public AdministrativeMember AdministrativeMember { get; set; }

        public EventExpense EventExpense { get; set; }


    }
}
