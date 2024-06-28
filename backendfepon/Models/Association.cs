﻿namespace backendfepon.Models
{
    public class Association
    {
        public int Association_Id { get; set; }
        public int State_Id { get; set; }
        public string Name { get; set; }
        public string Mission { get; set; }
        public string Vision { get; set; }
        public string Objective { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public State State { get; set; }
    }
}
