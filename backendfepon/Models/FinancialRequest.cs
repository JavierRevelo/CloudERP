﻿using backendfepon.ModelConfigurations;

namespace backendfepon.Models
{
    public class FinancialRequest
    {
        public int Request_Id { get; set; }
        public int Administrative_Member_Id { get; set; }
        public int Request_Status_Id { get; set; }
        public decimal Value { get; set; }
        public string Reason { get; set; }


        public AdministrativeMember AdministrativeMember { get; set; }

        public FinancialRequestState Financial_Request_State { get; set; }
    }
}