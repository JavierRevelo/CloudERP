namespace backendfepon.Models
{
    public class State
    {
        public int State_Id { get; set; }
        public string State_Name { get; set; }

        // Navigation property
        public ICollection<Product> Products { get; set; }
        public ICollection<ContributionPlan> ContributionPlans { get; set; }

        public ICollection<Event> Events { get; set; }
        public ICollection<Association> Associations { get; set; }
        public ICollection<AdministrativeMember> AdministrativesMembers { get; set; }
        public ICollection<Contributor> Contributors { get; set; }
        //public ICollection<CAccountingAccount> CAccountingAccounts { get; set; }
        public ICollection<InventoryMovement> InventoryMovements { get; set; }

        public ICollection<Provider> Providers { get; set; }
        public ICollection<FinancialRequest> FinancialRequests { get; set; }






    }
}
