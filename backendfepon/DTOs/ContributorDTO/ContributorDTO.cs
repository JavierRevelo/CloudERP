using backendfepon.Models;

namespace backendfepon.DTOs.ContributorDTO
{
    public class ContributorDTO
    {
        /*
        public int Contributor_Id { get; set; }
        public int Transaction_Id { get; set; }
        public string Plan_Name { get; set; }

        public decimal Plan_Economic_Value { get; set; }
        
        public string Student_FullName { get; set; }

        public string Student_Faculty { get; set; }

        public string Student_Career { get; set; }

        public string Student_Email { get; set; }
        */

        public int id { get; set; }
        public string date { get; set; }
        public int state_id { get; set; }
        public string name { get; set; }
        public string faculty { get; set; }
        public string career { get; set; }
        public string email { get; set; }
        public string plan { get; set; }
        public decimal price { get; set; }
        public string academicPeriod { get; set; }
    }
}

